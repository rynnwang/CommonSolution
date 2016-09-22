//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Beyova.ExceptionSystem;

//namespace Beyova.Diagnostic
//{
//    /// <summary>
//    /// Class NetworkDiagnosticReport.
//    /// </summary>
//    public class NetworkDiagnosticReport : IIdentifier
//    {
//        /// <summary>
//        /// Gets or sets the key.
//        /// </summary>
//        /// <value>The key.</value>
//        public Guid? Key { get; set; }

//        /// <summary>
//        /// Gets or sets the exception.
//        /// </summary>
//        /// <value>The exception.</value>
//        public ExceptionInfo Exception { get; set; }

//        /// <summary>
//        /// Gets or sets the created stamp.
//        /// </summary>
//        /// <value>The created stamp.</value>
//        public DateTime? CreatedStamp { get; set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="ServiceHealth" /> class.
//        /// </summary>
//        public NetworkDiagnosticReport()
//        {
//            this.Key = Guid.NewGuid();
//            this.CreatedStamp = DateTime.UtcNow;
//        }
//    }
//}
