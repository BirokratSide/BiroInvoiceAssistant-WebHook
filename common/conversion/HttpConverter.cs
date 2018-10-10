using si.birokrat.next.common.encoding;
using System.Text;
using System.Web;

namespace si.birokrat.next.common.conversion {
    public static class HttpConverter {
        public static string EncodeUrl(string value, Encoding encoding = null) {
            if (encoding == null) {
                encoding = Encoding.UTF8;
            }
            return HttpUtility.UrlEncode(value, encoding);
        }

        public static string EncodeUrl(string value, string encodingName = "utf-8") {
            var encoding = EncodingUtils.FindByName(encodingName);
            return EncodeUrl(value, encoding);
        }

        public static string EncodeUrl(string value, int encodingCodePage = 65001) {
            var encoding = EncodingUtils.FindByCodePage(encodingCodePage);
            return EncodeUrl(value, encoding);
        }

        public static string DecodeUrl(string value, Encoding encoding = null) {
            if (encoding == null) {
                encoding = Encoding.UTF8;
            }
            return HttpUtility.UrlDecode(value, encoding);
        }

        public static string DecodeUrl(string value, string encodingName = "utf-8") {
            var encoding = EncodingUtils.FindByName(encodingName);
            return DecodeUrl(value, encoding);
        }

        public static string DecodeUrl(string value, int encodingCodePage = 65001) {
            var encoding = EncodingUtils.FindByCodePage(encodingCodePage);
            return HttpUtility.UrlDecode(value, encoding);
        }
    }
}
