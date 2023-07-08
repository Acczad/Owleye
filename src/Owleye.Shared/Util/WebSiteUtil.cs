using System;
using System.Net;

namespace Owleye.Shared.Util
{
    public static class WebSiteUtil
    {
        public static bool IsUrlAlive(string url, int urlTimeout)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = urlTimeout;
                request.Method = "GET"; // TODO fix this, head maybe disallowed in some situations

                if (request.GetResponse() is HttpWebResponse response)
                {
                    var statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400)
                    {
                        return true;
                    }

                    if (statusCode >= 500 && statusCode <= 510)
                    {
                        return false;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                //TODO, log this.
            }
            return false;
        }

        public static Tuple<bool, int> IsUrlAliveWithStatus(string url, int urlTimeout)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Timeout = urlTimeout;
                request.Method = "HEAD"; // TODO fix this, head maybe disallowed in some situations

                if (request.GetResponse() is HttpWebResponse response)
                {
                    var statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400)
                    {
                        return Tuple.Create(true, statusCode);
                    }

                    return Tuple.Create(false, statusCode);

                }
            }
            catch (WebException ex)
            {
                var webResponse = ex.Response as HttpWebResponse;
                return Tuple.Create(false, (int)webResponse.StatusCode);
            }
            catch (Exception)
            {
                //TODO, log this.
            }
            return Tuple.Create(false, 500);
        }

    }
}
