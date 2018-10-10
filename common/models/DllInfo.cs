namespace si.birokrat.next.common.models {
    public class DllInfo : Info {
        public string Token { get; set; } = string.Empty;

        public bool Global { get; set; } = false;

        public string UserName { get; set; } = string.Empty;

        public int Fiscalization { get; set; } = 0;

        public string FilePath { get; set; } = string.Empty;
    }
}
