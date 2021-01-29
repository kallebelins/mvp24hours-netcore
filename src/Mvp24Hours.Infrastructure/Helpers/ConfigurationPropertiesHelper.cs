//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================

namespace Mvp24Hours.Infrastructure.Helpers
{
    public static class ConfigurationPropertiesHelper
    {
        #region [ Props ]

        private static int? _maxQtyByQueryPage;
        public static int MaxQtyByQueryPage
        {
            get
            {
                if (_maxQtyByQueryPage == null)
                {
                    int result;
                    if (int.TryParse((ConfigurationHelper.GetSettings("Mvp24Hours:Persistence:MaxQtyByQueryPage") ?? string.Empty), out result))
                    {
                        _maxQtyByQueryPage = result;
                    }
                    else
                    {
                        _maxQtyByQueryPage = 100;
                    }
                }
                return (int)_maxQtyByQueryPage;
            }
        }

        #endregion
    }
}
