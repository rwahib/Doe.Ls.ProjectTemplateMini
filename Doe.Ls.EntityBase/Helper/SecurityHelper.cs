using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doe.Ls.EntityBase.Helper {
    public class SecurityHelper {

        public string Encrypt(string value)
        {

            var util = new CryptoUtil();
            return util.Encrypt(value);

        }

        public string Decrypt(string value) {

            var util = new CryptoUtil();
            return util.Decrypt(value);

        }

    }
}
