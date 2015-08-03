using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace Selkie.NuGet.PackageToDependency
{
    public class ReadAllLines
    {
        protected string ReadAll([NotNull] string filename)
        {
            var sb = new StringBuilder();

            using (var sr = new StreamReader(filename))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }
    }
}