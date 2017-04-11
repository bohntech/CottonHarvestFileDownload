using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdAPI.Client.Rest
{
    public class HttpHeader
    {
        private String name;
        private String value;

        public HttpHeader(String name, String value)
        {
            this.name = name;
            this.value = value;
        }

        public String getName()
        {
            return name;
        }

        public String getValue()
        {
            return value;
        }

        /*@Override public boolean equals(final Object o) {
            if (this == o) { return true; }
            if (o == null || getClass() != o.getClass()) { return false; }

            final HttpHeader that = (HttpHeader) o;

            return name.equalsIgnoreCase(that.name) && value.equals(that.value);
        }

        @Override public int hashCode() {
            int result = name.toLowerCase().hashCode();
            result = 31 * result + value.hashCode();
            return result;
        }*/
    }
}
