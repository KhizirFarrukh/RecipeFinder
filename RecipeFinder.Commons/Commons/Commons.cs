using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinder.Common.Commons
{
    public class CustomMessage
    {
        public const string ADDED_SUCCESSFULLY = "{0} created successfully.";
        public const string DELETED_SUCCESSFULLY = "{0} deleted successfully.";
        public const string UPDATED_SUCCESSFULLY = "{0} updated successfully.";
        public const string NOT_FOUND = "{0} not found.";
        public const string NOT_UPLOADED = "{0} was not uploaded.";
        public const string ERROR_JSON_DESERIALIZING = "An error occurred while deserializing the json of {0}.";
    }

    public class DeveloperConstants
    {
        public const string ENDPOINT_PREFIX = "api/v1/[controller]";
        public const int PAGE_SIZE = 10;
    }

    public static class ResponseMessage
    {
        public const bool SUCCESS = true;
        public const bool FAILURE = false;
    }
}