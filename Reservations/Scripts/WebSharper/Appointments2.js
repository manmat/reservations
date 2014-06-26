(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,WebSharper,Formlet,Controls,Enhance,Data,Formlet1,Remoting,window,FormContainerConfiguration,Appointments2,MyPage,Auth,jQuery,Html,Default,List,T,JavaScript,alert;
 Runtime.Define(Global,{
  Appointments2:{
   Auth:{
    Main:function(url)
    {
     var arg10,name,pw,formlet;
     arg10=Controls.Input("");
     name=Enhance.WithTextLabel("Name",Enhance.WithValidationIcon(Data.Validator().IsNotEmpty("Don't forget your name!",arg10)));
     pw=Enhance.WithTextLabel("Password",Enhance.WithValidationIcon(Controls.Password("")));
     formlet=Data.$(Data.$(Formlet1.Return(function(name1)
     {
      return function(p)
      {
       return Remoting.Send("Appointments2:0",[name1,p]);
      };
     }),name),pw);
     return Formlet1.Run(function()
     {
      window.location=url;
      return url;
     },Enhance.WithCustomFormContainer(FormContainerConfiguration.get_Default(),Enhance.WithSubmitAndResetButtons(formlet)));
    }
   },
   Controls:{
    EntryPoint:Runtime.Class({
     get_Body:function()
     {
      return MyPage.Main(this.name,this.num);
     }
    }),
    Hekk:Runtime.Class({
     get_Body:function()
     {
      return MyPage.InitDateTime();
     }
    }),
    LoginEntry:Runtime.Class({
     get_Body:function()
     {
      return Auth.Main(this.url);
     }
    })
   },
   MyPage:{
    Form:function(n)
    {
     var arg10,formlet,name,x,email,date;
     arg10=Controls.Input(n);
     formlet=Data.Validator().IsNotEmpty("Don't forget your name!",arg10);
     name=Enhance.WithTextLabel("Name",Enhance.WithValidationIcon(formlet));
     x=Controls.Input("example@example.com");
     email=Enhance.WithTextLabel("E-mail",Enhance.WithValidationIcon((Data.Validator().IsEmail("Don't forget your email address!"))(x)));
     date=Enhance.WithCssClass("datetime",Enhance.WithTextLabel("Date",Controls.Input("")));
     return Data.$(Data.$(Data.$(Formlet1.Return(function(n1)
     {
      return function(a)
      {
       return function(d)
       {
        return{
         Name:n1,
         Email:a,
         Date:d
        };
       };
      };
     }),name),email),date);
    },
    InitDateTime:function()
    {
     jQuery(".datetime").find("input").datetimepicker({
      step:15,
      minDate:"0"
     });
     return Default.Div(Runtime.New(T,{
      $:0
     }));
    },
    Main:function(name,page)
    {
     var inputRecord,Description;
     JavaScript.Log(name);
     inputRecord=FormContainerConfiguration.get_Default();
     Description={
      $:1,
      $0:{
       $:0,
       $0:"Fill in the form"
      }
     };
     return Formlet1.Run(function(r)
     {
      return MyPage.select(page,r);
     },Enhance.WithCustomFormContainer(Runtime.New(FormContainerConfiguration,{
      Header:{
       $:1,
       $0:{
        $:0,
        $0:"Hello "+Global.String(name)
       }
      },
      Padding:inputRecord.Padding,
      Description:Description,
      BackgroundColor:inputRecord.BackgroundColor,
      BorderColor:inputRecord.BorderColor,
      CssClass:inputRecord.CssClass,
      Style:inputRecord.Style
     }),Enhance.WithSubmitAndResetButtons(MyPage.Form(name))));
    },
    select:function(page,r)
    {
     var matchValue,x;
     if(page.$==2)
      {
       matchValue=Remoting.Call("Appointments2:1",[r]);
       if(matchValue.$==1)
        {
         x=matchValue.$0;
         return alert("Name: "+x.Name+"\nEmail: "+x.Email);
        }
       else
        {
         return alert("Available");
        }
      }
     else
      {
       return page.$==1?Remoting.Call("Appointments2:2",[r])?alert("Reserved"):alert("Available"):page.$==3?Remoting.Send("Appointments2:4",[r]):page.$==4?Remoting.Send("Appointments2:5",[r]):Remoting.Call("Appointments2:3",[r])?alert("Success"):alert("Already reserved");
      }
    }
   }
  }
 });
 Runtime.OnInit(function()
 {
  WebSharper=Runtime.Safe(Global.IntelliFactory.WebSharper);
  Formlet=Runtime.Safe(WebSharper.Formlet);
  Controls=Runtime.Safe(Formlet.Controls);
  Enhance=Runtime.Safe(Formlet.Enhance);
  Data=Runtime.Safe(Formlet.Data);
  Formlet1=Runtime.Safe(Formlet.Formlet);
  Remoting=Runtime.Safe(WebSharper.Remoting);
  window=Runtime.Safe(Global.window);
  FormContainerConfiguration=Runtime.Safe(Enhance.FormContainerConfiguration);
  Appointments2=Runtime.Safe(Global.Appointments2);
  MyPage=Runtime.Safe(Appointments2.MyPage);
  Auth=Runtime.Safe(Appointments2.Auth);
  jQuery=Runtime.Safe(Global.jQuery);
  Html=Runtime.Safe(WebSharper.Html);
  Default=Runtime.Safe(Html.Default);
  List=Runtime.Safe(WebSharper.List);
  T=Runtime.Safe(List.T);
  JavaScript=Runtime.Safe(WebSharper.JavaScript);
  return alert=Runtime.Safe(Global.alert);
 });
 Runtime.OnLoad(function()
 {
  return;
 });
}());
