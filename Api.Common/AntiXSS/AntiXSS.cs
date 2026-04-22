namespace Api.Common.AntiXSS
{
    public class AntiXSS
    {
        public virtual AntiyXSSResult Scan(string taintedHtml, string filename)
        {
            Policy policy = Policy.FromFile(filename);

            var antiXSS = new AntiXSSDomScanner(policy);

            return antiXSS.Scan(taintedHtml);
        }

        public virtual AntiyXSSResult Scan(string taintedHtml, Policy policy)
        {
            var antiXSS = new AntiXSSDomScanner(policy);

            return antiXSS.Scan(taintedHtml);
        }

    }
}
