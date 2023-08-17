using System;
using Newtonsoft.Json;
namespace MattTools.Data;

public class RossumData
{


    #region Pagnation

    public class PagingObject<T>
    {
        public Pagination pagination { get; set; }
        public List<T> results { get; set; }
    }

    public class Pagination
    {
        public int total { get; set; }
        public int total_pages { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
    }

    #endregion

    #region Login
    public class LoginCache
    {
        public string username;
        public string key;
    }

    public class LoginForm
    {
        public string username { get; set; }
        public string password { get; set; }
        public string key { get; set; } = null;
    }

    public class LoginData
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginRespone
    {
        public string key { get; set; } = null;
        public string domain { get; set; } = null;
        public string error { get; set; } = null;
    }

    public class LoginBadRequest
    {
        public List<string> non_field_errors { get; set; }
    }
    #endregion

    #region Logout

    public class LogoutRespone
    {
        public bool loggedOut { get; set; }
        public string detail { get; set; }
        public string code { get; set; }
    }

    #endregion

    #region Queue
    public class QueueData
    {
        public string name;
        public int id;
    }

    public class QueueResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string workspace { get; set; }
        public object connector { get; set; }
        public List<object> hooks { get; set; }
        public List<object> webhooks { get; set; }
        public string schema { get; set; }
        public string inbox { get; set; }
        public List<object> users { get; set; }
        public string session_timeout { get; set; }
        public string rir_url { get; set; }
        public object rir_params { get; set; }
        public QueueCounts counts { get; set; }
        public double default_score_threshold { get; set; }
        public bool automation_enabled { get; set; }
        public string automation_level { get; set; }
        public string locale { get; set; }
        public QueueMetadata metadata { get; set; }
        public QueueSettings settings { get; set; }
        public bool use_confirmed_state { get; set; }
        public object document_lifetime { get; set; }
        public object dedicated_engine { get; set; }
        public string generic_engine { get; set; }
        public object delete_after { get; set; }
        public string status { get; set; }
    }

    public class QueueCounts
    {
        public int importing { get; set; }
        public int failed_import { get; set; }
        public int split { get; set; }
        public int to_review { get; set; }
        public int reviewing { get; set; }
        public int confirmed { get; set; }
        public int rejected { get; set; }
        public int exporting { get; set; }
        public int exported { get; set; }
        public int failed_export { get; set; }
        public int postponed { get; set; }
        public int deleted { get; set; }
        public int purged { get; set; }
    }

    public class QueueDashboardCustomization
    {
        public bool all_documents { get; set; }
        public bool to_review { get; set; }
        public bool postponed { get; set; }
        public bool confirmed { get; set; }
        public bool exported { get; set; }
        public bool rejected { get; set; }
        public bool deleted { get; set; }
    }

    public class QueueEmailNotifications
    {
        public object recipient { get; set; }
        public bool unprocessable_attachments { get; set; }
        public bool postponed_annotations { get; set; }
        public bool deleted_annotations { get; set; }
        public bool email_with_no_attachments { get; set; }
    }

    public class QueueMetadata
    {
        public string queueTemplate { get; set; }
    }

    public class QueueRejectionConfig
    {
        public bool enabled { get; set; }
    }

    public class QueueSettings
    {
        public string suggested_edit { get; set; }
        public QueueRejectionConfig rejection_config { get; set; }
        public List<QueueSuggestedRecipientsSource> suggested_recipients_sources { get; set; }
        public QueueEmailNotifications email_notifications { get; set; }
        public QueueDashboardCustomization dashboard_customization { get; set; }
    }

    public class QueueSuggestedRecipientsSource
    {
        public string source { get; set; }
    }
    #endregion

    #region Workspace

    public class WorkspaceData
    {
        public string name;
        public int id;
        public List<string> queueURL;
    }

    public class WorkspaceResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool autopilot { get; set; }
        public string organization { get; set; }
        public List<string> queues { get; set; }
        public WorkspaceMetaData metadata { get; set; }
    }

    public class WorkspaceMetaData
    {
        public string customerid { get; set; }
        public int source { get; set; }
        public List<string> authors { get; set; }
    }

    #endregion

    #region Document

    public class DocumentMetadata
    {
        [JsonProperty("customer-id")]
        public string customerid { get; set; }
        public int source { get; set; }
        public List<string> authors { get; set; }
    }

    public class DocumentResult
    {
        public int id { get; set; }
        public string url { get; set; }
        public string s3_name { get; set; }
        public object parent { get; set; }
        public object email { get; set; }
        public string mime_type { get; set; }
        public object creator { get; set; }
        public DateTime created_at { get; set; }
        public DateTime arrived_at { get; set; }
        public string original_file_name { get; set; }
        public string content { get; set; }
        public object attachment_status { get; set; }
        public DocumentMetadata metadata { get; set; }
        public List<string> annotations { get; set; }
    }

    #endregion

    #region Annotation

    public class AnnotationData
    {
        public bool selected;
        public int annotationID;
        public int docID;
        public int queueID;
        public string fileName;
        public string status;
        public string _status;
        public string uploadDate;
    }

    public class AnnotationResult
    {
        public string document { get; set; }
        public int id { get; set; }
        public string queue { get; set; }
        public string schema { get; set; }
        public List<object> relations { get; set; }
        public List<string> pages { get; set; }
        public string creator { get; set; }
        public object modifier { get; set; }
        public object assigned_at { get; set; }
        public object confirmed_at { get; set; }
        public DateTime created_at { get; set; }
        public object exported_at { get; set; }
        public object deleted_at { get; set; }
        public object modified_at { get; set; }
        public object purged_at { get; set; }
        public object rejected_at { get; set; }
        public object confirmed_by { get; set; }
        public object deleted_by { get; set; }
        public object exported_by { get; set; }
        public object purged_by { get; set; }
        public object rejected_by { get; set; }
        public string status { get; set; }
        public string rir_poll_id { get; set; }
        public List<object> messages { get; set; }
        public string url { get; set; }
        public string content { get; set; }
        public double time_spent { get; set; }
        public AnnotationMetadata metadata { get; set; }
        public bool automated { get; set; }
        public object suggested_edit { get; set; }
        public List<object> related_emails { get; set; }
        public object email { get; set; }
        public string automation_blocker { get; set; }
        public object email_thread { get; set; }
        public bool has_email_thread_with_replies { get; set; }
        public bool has_email_thread_with_new_replies { get; set; }
        public string organization { get; set; }
        public bool automatically_rejected { get; set; }
        public AnnotationMetadata prediction { get; set; }
    }

    public class AnnotationPrediction
    {
        public string source { get; set; }
        public object version { get; set; }
    }

    public class AnnotationMetadata
    {
        public string sampleDocument { get; set; }
    }
    #endregion

}

