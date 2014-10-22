﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class Profile : ISerializable
    {
        public ProtectedValue LockTime { get; private set; }
        public ProtectedValue SyncTime { get; private set; }
        
        private static Profile _instance;
        private ProtectedValue _patternHash;
        private ProtectedValue _ptoken;
        private ProtectedValue _ptokenHash;
        private static readonly ProtectedValue InternalKey = new ProtectedValue(Md5.Encode("64b499e339d2eacd2952b9ccf6f69549"));
        private static readonly ProtectedValue PlayerPrefsKey = new ProtectedValue(Md5.Encode("c6401824b1b2e71c2b2a67075f65a430"));
        private readonly List<CardSlot> _cards = new List<CardSlot>();

        private Profile()
        {
        }

        public static Profile Instance
        {
            get { return _instance ?? (_instance = Load()); }
        }

        #region Save / Load

        public Profile(SerializationInfo info, StreamingContext context)
        {
            foreach (var entry in info)
            {
                var value = (string) entry.Value;

                switch (entry.Name)
                {
                    case "LockTime":
                        LockTime = Serializer.Deserialize<ProtectedValue>(value);
                        break;
                    case "SyncTime":
                        SyncTime = Serializer.Deserialize<ProtectedValue>(value);
                        break;
                    case "_patternHash":
                        _patternHash = Serializer.Deserialize<ProtectedValue>(value);
                        break;
                    case "_ptoken":
                        _ptoken = Serializer.Deserialize<ProtectedValue>(value);
                        break;
                    case "_ptokenHash":
                        _ptokenHash = Serializer.Deserialize<ProtectedValue>(value);
                        break;
                    case "_cards":
                        _cards = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Select(i => Serializer.Deserialize<CardSlot>(i)).ToList();
                        break;
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (LockTime != null)
            {
                info.AddValue("LockTime", Serializer.Serialize(LockTime));
            }

            if (SyncTime != null)
            {
                info.AddValue("SyncTime", Serializer.Serialize(SyncTime));
            }

            if (_patternHash != null)
            {
                info.AddValue("_patternHash", Serializer.Serialize(_patternHash));
            }

            if (_ptoken != null)
            {
                info.AddValue("_ptoken", Serializer.Serialize(_ptoken));
            }

            if (_ptokenHash != null)
            {
                info.AddValue("_ptokenHash", Serializer.Serialize(_ptokenHash));
            }

            if (_cards.Count > 0)
            {
                info.AddValue("_cards", string.Join(Environment.NewLine, _cards.Select(i => Serializer.Serialize(i)).ToArray()));
            }
        }

        public void Reset()
        {
            var ptoken = _ptoken.Copy();
            var ptokenHash = _ptokenHash.Copy();

            if (PlayerPrefs.HasKey(PlayerPrefsKey.String))
            {
                PlayerPrefs.DeleteKey(PlayerPrefsKey.String);
                PlayerPrefs.Save();
            }

            _instance = new Profile { _ptoken = ptoken, _ptokenHash = ptokenHash };
        }

        private static Profile Load()
        {
            //PlayerPrefs.DeleteKey(PlayerPrefsKey.String); // TODO:
            //PlayerPrefs.Save();

            return PlayerPrefs.HasKey(PlayerPrefsKey.String) ? Load(PlayerPrefs.GetString(PlayerPrefsKey.String)) : new Profile();
        }

        private static Profile Load(string encrypted)
        {
            return Serializer.Deserialize<Profile>(AES.Decrypt(encrypted, InternalKey.String));
        }

        private void Save()
        {
            PlayerPrefs.SetString(PlayerPrefsKey.String, Encoding.UTF8.GetString(Encrypt()));
            PlayerPrefs.Save();
        }

        #endregion

        #region Lock

        public void Lock(DateTime time)
        {
            LockTime = new ProtectedValue(time);
            Save();
        }

        public void Unlock()
        {
            LockTime = null;
            Save();
        }

        #endregion

        #region Pattern

        public bool PatternExists()
        {
            return _patternHash != null;
        }

        public bool CheckPattern(ProtectedValue pattern)
        {
            return Md5.Encode(pattern.String).Equals(_patternHash.String, StringComparison.InvariantCultureIgnoreCase);
        }

        public void CreatePattern(ProtectedValue pattern)
        {
            _patternHash = Md5.Encode(pattern);

            try
            {
                if (_ptokenHash != Md5.Encode(AES.Decrypt(_ptoken, pattern)))
                {
                    throw new Exception();
                }
            }
            catch
            {
                _ptoken = _ptokenHash = null;
            }

            Save();
        }

        public void ChangePattern(ProtectedValue pattern, ProtectedValue newPattern)
        {
            _patternHash = Md5.Encode(newPattern);

            if (_ptoken == null)
            {
                _cards.ForEach(i => i.Data = Recrypt(i.Data, pattern, newPattern));
            }
            else
            {
                _ptoken = Recrypt(_ptoken, pattern, newPattern);
            }

            Save();
        }

        #endregion

        #region Cards

        public bool CardExists(ProtectedValue slot)
        {
            return _cards.Any(i => i.Slot.Int == slot.Int && i.Data != null);
        }

        public bool PhantomExists(ProtectedValue slot)
        {
            return _cards.Any(i => i.Slot.Int == slot.Int && i.Data == null);
        }

        public void CreateCard(ProtectedValue slot, ProtectedValue card, ProtectedValue pattern)
        {
            var c = new CardSlot
            {
                Slot = slot,
                Data = AES.Encrypt(card, GetEncryptionKey(pattern)),
                Timestamp = new ProtectedValue(DateTime.UtcNow)
            };

            if (PhantomExists(slot))
            {
                _cards[GetIndex(slot)] = c;
            }
            else
            {
                _cards.Add(c);
            }

            Save();
        }

        public ProtectedValue ProtectCard(CardData card)
        {
            return new ProtectedValue(AES.Encrypt(Serializer.Serialize(card), InternalKey.String));
        }

        public void DeleteCard(ProtectedValue slot, bool save = true)
        {
            var i = GetIndex(slot);

            _cards[i].Data = null;

            if (save)
            {
                Save();
            }
        }

        public void UpdateCard(ProtectedValue slot, ProtectedValue card, ProtectedValue pattern)
        {
            var index = GetIndex(slot);

            _cards[index].Data = AES.Encrypt(card, GetEncryptionKey(pattern));
            _cards[index].Timestamp = new ProtectedValue(DateTime.UtcNow);
            Save();
        }

        public bool Merge(string profile)
        {
            return Sync.Merge(this, profile);
        }

        public CardData GetCard(ProtectedValue slot, ProtectedValue pattern)
        {
            return Serializer.Deserialize<CardData>(AES.Decrypt(AES.Decrypt(_cards[GetIndex(slot)].Data, GetEncryptionKey(pattern)), InternalKey).String);
        }

        public List<PartialCardData> GetCards(ProtectedValue pattern)
        {
            return Instance._cards.Where(i => i.Data != null).Select(i => GetCard(i.Slot, pattern)).Cast<PartialCardData>().ToList();
        }

        public int CountCards()
        {
            return Instance._cards.Count;
        }

        private int GetIndex(ProtectedValue slot)
        {
            return _cards.IndexOf(_cards.Single(i => i.Slot.Int == slot.Int));
        }

        #endregion

        #region Security

        public byte[] Encrypt()
        {
            return Encoding.UTF8.GetBytes(AES.Encrypt(Serializer.Serialize(this), InternalKey.String));
        }

        public void CreateToken(ProtectedValue ptoken, ProtectedValue pattern)
        {
            _ptoken = AES.Encrypt(ptoken, pattern);
            _ptokenHash = Md5.Encode(ptoken);

            foreach (var card in _cards.Where(c => c.Data != null))
            {
                card.Data = Recrypt(card.Data, pattern, ptoken);
            }

            Save();
        }

        private ProtectedValue GetEncryptionKey(ProtectedValue pattern)
        {
            Debug.Log("_ptokenHash=" + (_ptokenHash == null ? "null" : _ptokenHash.String)); // TODO:

            return _ptoken == null ? pattern : AES.Decrypt(_ptoken, pattern);
        }

        private static ProtectedValue Recrypt(ProtectedValue value, ProtectedValue pattern, ProtectedValue newPattern)
        {
            Debug.Log(value);
            Debug.Log(pattern);
            Debug.Log(newPattern);

            return new ProtectedValue(AES.Encrypt(AES.Decrypt(value.String, pattern.String), newPattern.String));
        }

        #endregion

        #region Sync

        public bool Premium
        {
            get { return _ptoken != null; }
        }

        public void SaveSyncTime()
        {
            SyncTime = new ProtectedValue(DateTime.UtcNow);
            Save();
        }

        public static class Sync
        {
            public static byte[] Merge(string localData, string serverData)
            {
                var profile = Load(localData);

                Merge(profile, serverData);

                return profile.Encrypt();
            }

            public static bool Merge(Profile profile, string data)
            {
                var p = Serializer.Deserialize<Profile>(AES.Decrypt(data, InternalKey.String));
                var updated = false;

                if (profile._ptokenHash != p._ptokenHash)
                {
                    if (profile._patternHash == p._patternHash)
                    {
                        profile._ptoken = p._ptoken.Copy();
                        profile._ptokenHash = p._ptokenHash.Copy();
                    }
                    else
                    {
                        throw new Exception("%TokensNotEqual%");
                    }
                }

                foreach (var card in p._cards)
                {
                    var cardExists = profile.CardExists(card.Slot);
                    var phantomExists = profile.PhantomExists(card.Slot);

                    if (cardExists || phantomExists)
                    {
                        var i = profile.GetIndex(card.Slot);

                        if (card.Timestamp.DateTime < profile._cards[i].Timestamp.DateTime) continue;

                        if (card.Data == null)
                        {
                            if (cardExists)
                            {
                                profile.DeleteCard(profile._cards[i].Slot, save: false); updated = true;
                            }
                        }
                        else
                        {
                            if (card.Timestamp.DateTime > profile._cards[i].Timestamp.DateTime)
                            {
                                profile._cards[i] = card.Copy(); updated = true;
                            }
                        }
                    }
                    else
                    {
                        profile._cards.Add(card.Copy()); updated = true;
                    }
                }

                return updated;
            }
        }

        #endregion
    }
}