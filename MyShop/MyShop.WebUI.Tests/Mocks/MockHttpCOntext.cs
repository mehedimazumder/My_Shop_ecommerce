﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockRequest request;
        private MockResponse response;
        private HttpCookieCollection cookies;

        public MockHttpContext()
        {
            cookies = new HttpCookieCollection();
            this.request = new MockRequest(cookies);
            this.response = new MockResponse(cookies);
        }

        public override HttpRequestBase Request
        {
            get { return request; }
        }
        public override HttpResponseBase Response
        {
            get { return response; }
        }
    }

    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection _cookies;

        public MockResponse(HttpCookieCollection cookies)
        {
            this._cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get { return _cookies; }
        }

    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection _cookies;

        public MockRequest(HttpCookieCollection cookies)
        {
            this._cookies = cookies;
        }

        public override HttpCookieCollection Cookies
        {
            get { return _cookies; }
        }

    }
}