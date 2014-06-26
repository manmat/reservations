declare module Appointments2 {
    module Skin {
        interface Page {
            Title: string;
            Body: __ABBREV.__List.T<__ABBREV.__Html.Element<__ABBREV.__Web.Control>>;
        }
    }
    module Controls {
        interface EntryPoint {
            get_Body(): __ABBREV.__Html1.IPagelet;
        }
        interface Hekk {
            get_Body(): __ABBREV.__Html1.IPagelet;
        }
        interface LoginEntry {
            get_Body(): __ABBREV.__Html1.IPagelet;
        }
    }
    module MyPage {
        var Form : {
            (n: string): __ABBREV.__Data.Formlet<any>;
        };
        var select : {
            (page: __ABBREV.__Server.Pages, r: any): void;
        };
        var Main : {
            (name: string, page: __ABBREV.__Server.Pages): __ABBREV.__Html1.IPagelet;
        };
        var InitDateTime : {
            (): __ABBREV.__Html1.IPagelet;
        };
    }
    module Server {
        interface Pages {
        }
        interface Reserv {
            Name: string;
            Email: string;
            Date: string;
        }
    }
    module Auth {
        interface User {
            Name: string;
            IsAdmin: boolean;
        }
        var Main : {
            <_M1>(url: _M1): __ABBREV.__Html1.IPagelet;
        };
    }
    interface Action {
    }
    interface Website {
    }
}
declare module __ABBREV {
    
    export import __List = IntelliFactory.WebSharper.List;
    export import __Html = IntelliFactory.Html.Html;
    export import __Web = IntelliFactory.WebSharper.Web;
    export import __Html1 = IntelliFactory.WebSharper.Html;
    export import __Data = IntelliFactory.WebSharper.Formlet.Data;
    export import __Server = Appointments2.Server;
}
