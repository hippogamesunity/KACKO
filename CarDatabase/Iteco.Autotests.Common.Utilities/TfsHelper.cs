using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Iteco.Autotests.Common.Utilities
{
    public class TfsHelper
    {
        private static TfsHelper _instance;
        private readonly TfsTeamProjectCollection _tfsTeamProjectCollection;

        private TfsHelper()
        {
            _tfsTeamProjectCollection = new TfsTeamProjectCollection(Constants.TfsCollectionUri, Constants.TfsAdminCredentials);
            _tfsTeamProjectCollection.EnsureAuthenticated();
        }

        public static TfsHelper Instance
        {
            get { return _instance ?? (_instance = new TfsHelper()); }
        }

        public List<string> GetActualBugNamesForTest(int testCaseId)
        {
            return GetActualBugsForTest(testCaseId).Select(i => string.Format("{0} {1} (Severity: {2})", i.Id, i.Title, i["Severity"])).ToList();
        }

        public List<string> GetActualBugNamesForProject(string project, string iteration)
        {
            return GetActualBugsForProject(project, iteration).Select(i => string.Format("{0} {1} (Severity: {2})", i.Id, i.Title, i["Severity"])).ToList();
        }

        public List<int> GetActualBugIdsForTest(int testCaseId)
        {
            return GetActualBugsForTest(testCaseId).Select(i => i.Id).ToList();
        }

        public bool BugActive(int id)
        {
            return _tfsTeamProjectCollection.GetService<WorkItemStore>().GetWorkItem(id).State != "Closed";
        }

        private IEnumerable<WorkItem> GetActualBugsForTest(int testCaseId)
        {
            const string closedState = "Closed";
            const string bugTypeName = "Bug";

            var workItemStore = _tfsTeamProjectCollection.GetService<WorkItemStore>();
            var workItem = workItemStore.GetWorkItem(testCaseId);
            var relatedWorkItemIds = workItem.Links.Cast<Link>()
                .Where(i => i.ArtifactLinkType.Name == "Related Workitem").Cast<RelatedLink>().Select(i => i.RelatedWorkItemId).ToList();
            var bugs = relatedWorkItemIds.
                Select(i => workItemStore.GetWorkItem(i)).
                Where(i => i.State != closedState && i.Type.Name == bugTypeName);

            return bugs;
        }

        private IEnumerable<WorkItem> GetActualBugsForProject(string project, string iteration)
        {
            var workItemStore = _tfsTeamProjectCollection.GetService<WorkItemStore>();
            var query = string.Format(@"Select [Id], [Title] From WorkItems
                Where [Work Item Type] = 'Bug'
                And [State] <> 'Closed'
                And [Team Project] == '{0}'
                And [Area Path] Under '{1}'", project, iteration);
            var bugs = workItemStore.Query(query).Cast<WorkItem>();

            return bugs;
        }
    }
}