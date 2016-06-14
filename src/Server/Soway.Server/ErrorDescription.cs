using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service
{
    public static class ErrorDescription
    {
        // TODO: Error code will use less than 1000 when system errors.
        public const int CODE_ARGUMENT_ERROR = 299;
        public const string MESSAGE_ARGUMENT_ERROR = "Parameter error.";

        public const int CODE_RUN_OPERATION_ERROR = 300;
        public const string MESSAGE_RUN_OPERATION_ERROR = "Run operation error. ";

        public const int CODE_SYSTEM_ERROR = 500;
        public const string MESSAGE_SYSTEM_ERROR = "System busy. ";

        // TODO: Error code will increase from 10001 to define custom errors.
        public const int CODE_AUTHENTICATE_FAIL = 10001;
        public const string MESSAGE_AUTHENTICATE_FAIL = "Authenticate fail.";


        public const int CODE_AUTHENTICATE_APPNULL = 10002;
        public const string MESSAGE_AUTHENTICATE_APPNULL = "No application define.";

        public const int CODE_AUTHENTICATE_APPUNAUTH = 10003;
        public const string MESSAGE_AUTHENTICATE_PUUUNAUTH = "Wrong application sec.";

        public static readonly int TOKEN_INVALIDAT = 10004;
        public static readonly string TOKEN_INVALIDAT_MSG = "Token invalidat.";

        public static readonly int SET_SESSION_FAIL = 10005;
        public static readonly string SET_SESSION_FAIL_MSG = "Set session fail.";

        public static readonly int CHECK_CODE_ERROR = 10006;
        public static readonly string CHECK_CODE_ERROR_MSG = "Check code error.";


        public static readonly int DB_SELECT_ERROR = 10007;
        public static readonly string DB_SELECT_ERROR_MSG = "Unauthorized DataBase .";


        public const int CODE_AUTHENTICATE_NOUSER = 10008;
        public const string MESSAGE_AUTHENTICATE_NOUSER = "No User Info.";

    }
}