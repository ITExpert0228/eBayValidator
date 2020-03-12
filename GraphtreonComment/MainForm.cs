using System.Windows.Forms;
using System;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using Gecko;
using System.Text;
using ThangDC.Core.Entities;
using System.Web.Script.Serialization;
using Gecko.JQuery;
using Gecko.DOM;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Data;
namespace MGBot
{
    using StringList = List<string>;

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class MainForm : Form
    {
        #region static variable

        private bool IsRunning = false;
        private bool IsStop = true;
        private TabPage currentTab = null;

        //WebBrowser Main
        private WebBrowser wbMain;
        private GeckoHtmlElement htmlElm;

        private string LastScriptFile = "obeygiant.csr";
        public string Version = "1.1.4";

        public string MaxWait = string.Empty;

        private bool IsBreakSleep = false;
        private Random rnd = new Random();

        private StringList urlList = new StringList();
        private StringList keywordList = new StringList();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Int32 SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        #endregion
        public MainForm()
        {
            InitializeComponent();
        }

        public void tabnew()
        {
            TabPage tab = new TabPage("");
            tabMain.Controls.Add(tab);
            tab.Dock = DockStyle.Fill;
            currentTab = tab;
            tabMain.SelectedTab = currentTab;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CallBackWinAppWebBrowser();
            var path = Application.StartupPath + "\\Firefox";
            Xpcom.Initialize(path);
            PromptFactory.PromptServiceCreator = () => new FilteredPromptService();
            LastScriptFile = Application.StartupPath + "\\obeygiant.query";
            LoadScript(null);

            if (System.IO.File.Exists(LastScriptFile))
            {
                sleep(1, false);
            }
            MaxWait = (ConfigurationManager.AppSettings["MaxWait"] != null ? ConfigurationManager.AppSettings["MaxWait"] : string.Empty);


            radLogin.Select();
            Load_AccountList();
            Load_UAlist();
            // LoadUrls();
            //go("about:config");
            //goLogin();
            go("https://reg.ebay.com/reg/PartialReg");
        }
        public void LoadScript(string[] args)
        {
            if (args == null) return;
            if (args.Length > 0)
            {
                var path = args[0];
                if (!string.IsNullOrEmpty(path))
                {
                    if (System.IO.File.Exists(path))
                    {
                        sleep(1, false);
                        string code = read(path);
                        RunCode(code);
                    }
                }
            }
        }

        private object GetCurrentWB()
        {
            if (tabMain.SelectedTab != null)
            {
                if (tabMain.SelectedTab.Controls.Count > 0)
                {
                    Control ctr = tabMain.SelectedTab.Controls[0];
                    if (ctr != null)
                    {
                        return ctr as object;
                    }
                }
            }
            return null;
        }

        delegate void StringArgDelegate(string scriptString);
        public void RunCode(string scriptString)
        {
            if(wbMain.InvokeRequired)
            {
                StringArgDelegate d = new StringArgDelegate(RunCode);
                this.Invoke(d, scriptString);
            }
            else
            {
                if (IsStop)
                {
                    IsStop = false;
                    wbMain.Document.InvokeScript("UnAbort");
                    if (!string.IsNullOrEmpty(scriptString))
                    {
                        ExcuteJSCode(scriptString);
                    }
                }
                else
                {
                    IsStop = true;
                    wbMain.Document.InvokeScript("Abort");
                }

                IsStop = true;
            }
        }
        private void ExcuteJSCode(string code)
        {
            ExcuteJSCodeWebBrowser(code);
        }

        #region TextBox and Go Event Functions

        nsIMemory _memoryService = null;
        public void go(string url)
        {
            if (currentTab == null)
            {
                tabnew();
            }

            //WebBrowser
            if (!url.StartsWith("/"))
            {
                if (currentTab.Controls.Count > 0)
                {
                    Control ctr = currentTab.Controls[0];
                    if (ctr != null)
                    {
                        var wb = (GeckoWebBrowser)ctr;
                        wb.Stop();
                        wb.ProgressChanged -= wbBrowser_ProgressChanged;
                        wb.Navigated -= wbBrowser_Navigated;
                        wb.DocumentCompleted -= wbBrowser_DocumentCompleted;
                        wb.CanGoBackChanged -= wbBrowser_CanGoBackChanged;
                        wb.CanGoForwardChanged -= wbBrowser_CanGoForwardChanged;
                        wb.ShowContextMenu -= new EventHandler<GeckoContextMenuEventArgs>(wbBrowser_ShowContextMenu);
                        wb.DomContextMenu -= wbBrowser_DomContextMenu;
                        wb.Dispose();
                        wb = null;

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (_memoryService == null)
                        {
                            _memoryService = Xpcom.GetService<nsIMemory>("@mozilla.org/xpcom/memory-service;1");
                        }
                        _memoryService.HeapMinimize(false);
                    }
                    currentTab.Controls.Clear();
                }
                GoWebBrowser(url);
            }
            else
            {
                GoWebBrowserByXpath(url);
            }
        }

        #endregion
        #region WebBrowser
        public void CallBackWinAppWebBrowser()
        {
            wbMain = new WebBrowser();
            wbMain.ObjectForScripting = this;
            wbMain.ScriptErrorsSuppressed = true;
            wbMain.DocumentText = @"<html>
                                        <head>
                                            <script type='text/javascript'>
                                                var isAborted = false;

                                                function UnAbort() {isAborted = false; window.external.UnAbort();}
                                                function Abort() {isAborted = true; release(); window.external.Abort();}
                                                function CheckAbort() {if(isAborted == true) { window.external.Abort(); throw new Error('Aborted');} }

                                                /*var isAborted = false;
                                                function UnAbort() {isAborted = false;}
                                                function Abort() {isAborted = true;}
                                                function CheckAbort() {if(isAborted == true) throw new Error('Aborted');}*/

                                                function stringtoXML(data){ if (window.ActiveXObject){ var doc = new ActiveXObject('Microsoft.XMLDOM'); doc.async='false'; doc.loadXML(data); } else { var parser = new DOMParser(); var doc = parser.parseFromString(data,'text/xml'); }	return doc; }

                                                /* Open new tab */

                                                function release() { CheckAbort(); window.external.ReleaseMR();  }

                                                function countNodes(xpath) { CheckAbort(); return window.external.countNodes(xpath); } 

                                                function tabnew() { CheckAbort(); window.external.tabnew();}
                                                /* Close current tab  */
                                                function tabclose() { CheckAbort(); window.external.tabclose();}
                                                /* Close all tab  */
                                                function tabcloseall() { CheckAbort(); window.external.tabcloseall();}
                                                /* Go to website by url or xpath  */
                                                function go(a) { CheckAbort(); window.external.go(a);}
                                                
                                                function goWithProxy(url, proxyUrl){ CheckAbort(); window.external.goWithProxy(url, proxyUrl); }

                                                function back() { CheckAbort(); window.external.Back(); }
                                                function next() { CheckAbort(); window.external.Next(); }
                                                function reload() { CheckAbort(); window.external.Reload(); }
                                                function stop() { CheckAbort(); window.external.Stop(); }

                                                /* Sleep with a = miliseconds to sleep, b = true if wait until browser loading finished, b = false wait until timeout miliseconds  */
                                                function sleep(a, b) { CheckAbort(); window.external.sleep(a,b);}
                                                /* Quit application  */
                                                function exit() { CheckAbort(); window.external.exit();}
                                                /* Click by xpath  */
                                                function click(a) { CheckAbort(); window.external.click(a);}
                                                /* write a log to preview, a = content of log  */
                                                function log(a) { CheckAbort(); window.external.log(a);}
                                                /* clear log on the preview  */
                                                function clearlog() { CheckAbort(); window.external.clearlog();}
                                                /* extract data from xpath  */
                                                //function extract(a) {CheckAbort(); return window.external.extract(a);}
                                                function extract(xpath, type) {CheckAbort(); return window.external.extract(xpath, type);}

                                                function extractUntil(xpath, type){ CheckAbort(); return window.external.extractUntil(xpath, type); }

                                                function filliframe(title, value) { CheckAbort(); window.external.filliframe(title, value); }                                                

                                                /* fill xpath by value, a = xpath, b = value  */
                                                function fill(a,b) { CheckAbort(); window.external.fill(a,b);}
                                                /* convert extract string to object  */

                                                /*function filldropdown(a, b) { CheckAbort(); window.external.filldropdown(a, b); }*/
                                                function filldropdown(xpath, value) { CheckAbort(); window.external.filldropdown(xpath, value); }
                                                function toObject(a) {CheckAbort(); var wrapper= document.createElement('div'); wrapper.innerHTML= a; return wrapper;}
                                                function blockFlash(isBlock) { CheckAbort(); window.external.BlockFlash(isBlock); }

                                                /* browser get all link in the area of xpath, it will stop until program go all of link , a = xpath */
                                                function browser(a) {CheckAbort(); window.external.browser(a);}
                                                /* reset list website to unread so program can go back and browser continue */
                                                function resetlistwebsite() {CheckAbort(); window.external.ResetListWebsite();}
                                                /* take a snapshot from current website on current tab, a = location to save a snapshot */

                                                function takesnapshot(a) {CheckAbort(); window.external.TakeSnapshot(a);}
                                                /* reconigze text of image from url, a = url of image  */
                                                function imageToText(xpath, language) { CheckAbort(); return window.external.imgToText(xpath, language);}
                                                /* set value to file upload (not work in ie)  */
                                                function fileupload(a,b){CheckAbort(); window.external.FileUpload(a,b);}

                                                /* create folder, a = location  */
                                                function createfolder(a) { CheckAbort(); window.external.createfolder(a);}
                                                /* download file from url, a = url to download, b = location where file located  */
                                                function download(a,b) {CheckAbort(); window.external.download(a,b);}

                                                function downloadWebsite(url) { CheckAbort(); return window.external.DownloadWebsite(url); } 

                                                function getfiles(a) { CheckAbort(); return window.external.getfiles(a); }
                                                function getfolders(a) { CheckAbort(); return window.external.getfolders(a); }

                                                /* read a file, a = location of file  */
                                                function read(a) { CheckAbort(); return window.external.read(a);}
                                                /* save file, a = content of file, b = location of file to save, c = is file override (true: fill will be override, false: not override)  */
                                                function save(a,b,c) { CheckAbort(); return window.external.save(a,b,c);}
                                                /* remove a file, a = location of file will be removed */
                                                function remove(a) { CheckAbort(); window.external.remove(a);}
                                                function removefolder(a) {CheckAbort(); window.external.removefolder(a);}
                                                
                                                function copyfolder(a,b,c) {CheckAbort(); window.external.copyFolder(a,b,c);}
                                                function copyfile(a,b,c) {CheckAbort(); window.external.copyFile(a,b,c);}

                                                function replacetextinfile(a, b, c) { CheckAbort(); window.external.replaceTextinFile(a,b,c); }

                                                function explorer(a) { CheckAbort(); window.external.explorer(a); }

                                                /* run code from string, a = code to run  */
                                                function excute(a) { CheckAbort(); window.external.excute(a);}

                                                function logoff() { CheckAbort(); window.external.logoff();} 
                                                function lockworkstation() {CheckAbort(); window.external.lockworkstation();} 
                                                function forcelogoff() { CheckAbort(); window.external.forcelogoff();} 
                                                function reboot() { CheckAbort(); window.external.reboot();} 
                                                function shutdown() { CheckAbort(); window.external.shutdown();} 
                                                function hibernate() { CheckAbort(); window.external.hibernate();} 
                                                function standby() { CheckAbort(); window.external.standby();} 


                                                /* run application, a = location of application */
                                                function runcommand(path, parameters) { CheckAbort(); window.external.runcommand(path, parameters); }

                                                function createtask(a,b,c,d,e,f) { CheckAbort(); window.external.createtask(a,b,c,d,e,f); }
                                                function removetask(a) { CheckAbort(); window.external.removetask(a);}

                                                function generatekeys() { CheckAbort(); window.external.generatekeys();}
                                                function encrypt(a, b) { CheckAbort(); return window.external.encrypt(a, b);}
                                                function decrypt(a, b) { CheckAbort(); return window.external.decrypt(a, b);}

                                                function showpicture(a,b) { CheckAbort(); window.external.showimage(a,b); }
                                                function savefilterimage(a) { CheckAbort(); window.external.savefilterimage(a); }

                                                function writetextimage(a, b) {CheckAbort(); window.external.writetextimage(a,b); } 

                                                function getcurrenturl() {CheckAbort(); return window.external.getCurrentUrl();}

                                                function scrollto(a) {CheckAbort(); window.external.scrollto(a); }

                                                function getheight() { CheckAbort(); return window.external.getheight(); }

                                                function gettitle() { CheckAbort(); return window.external.gettitle(); } 

                                                function getlinks(a) { CheckAbort(); return window.external.getlinks(a); } 

                                                function getCurrentContent() { CheckAbort(); return window.external.getCurrentContent(); } 

                                                function getCurrentPath() { CheckAbort(); return window.external.getCurrentPath(); } 

                                                function checkelement(a) { CheckAbort(); return window.external.checkelement(a);}

                                                function readCellExcel(a, b, c, d) { CheckAbort(); return window.external.readCellExcel(a,b,c,d);}

                                                function writeCellExcel(a, b, c, d) { CheckAbort(); window.external.writeCellExcel(a,b,c,d); }

                                                function replaceMsWord(a, b, c, d) { CheckAbort(); window.external.replaceMsWord(a,b,c,d); } 

                                                function loadHTML(a) { CheckAbort(); window.external.loadHTML(a); }" +

                                                "function textToJSON(a) { CheckAbort(); var b = eval(\"(\" + window.external.textToJSON(a) + \")\"); return b; }" +

                                                @"function getCurrentLogin() { return textToJSON(window.external.getCurrentUser());}

                                                function login(a, b) { return window.external.login(a,b); }

                                                function register(a, b, c, d) { return window.external.register(a, b, c, d);}

                                                function getAccount(a) { CheckAbort(); var b = window.external.GetAccount(a); if(b == '') return ''; else return textToJSON(b); }

                                                function captchaborder(a,b) { CheckAbort(); window.external.CaptchaBorder(a,b); } 

                                                function saveImageFromElement(a,b) { CheckAbort(); window.external.SaveImageFromElement(a,b);}

                                                function getControlText(a,b,c) { CheckAbort(); return window.external.GetControlText(a,b,c); }

                                                function setControlText(a,b,c,d) { CheckAbort(); window.external.SetControlText(a,b,c,d); }

                                                function clickControl(a,b,c) { CheckAbort(); window.external.ClickControl(a,b,c); } 

                                                function getCurrentMouseX() { CheckAbort(); return window.external.GetCurrentMouseX(); } 

                                                function getCurrentMouseY() { CheckAbort(); return window.external.GetCurrentMouseY(); } 

                                                function MouseDown(a,b) { CheckAbort(); window.external.Mouse_Down(a,b); }

                                                function MouseUp(a,b) { CheckAbort(); window.external.Mouse_Up(a,b); }

                                                function MouseClick(a,b) { CheckAbort(); window.external.Mouse_Click(a,b); }

                                                function MouseDoubleClick(a,b) { CheckAbort(); window.external.Mouse_Double_Click(a,b); }

                                                function MouseMove(a,b,c,d) {CheckAbort(); window.external.Mouse_Show(a,b,c,d); }

                                                function MouseWheel(a,b) { CheckAbort(); window.external.Mouse_Wheel(a,b); }

                                                function KeyDown(a,b) { CheckAbort(); window.external.Key_Down(a,b); }

                                                function KeyUp(a,b) { CheckAbort(); window.external.Key_Up(a,b); }

                                                function sendText(a) { CheckAbort(); window.external.sendText(a); }

                                                function Reload() { CheckAbort(); window.external.Reload(); }

                                                function sendEmail(name, email, subject, content) { CheckAbort(); return window.external.sendEmail(name, email, subject, content); }" +

                                                "function getAccountBy(name) { CheckAbort(); var a = window.external.GetAccountBy(name); if(a != '') { return eval(\"(\" + a + \")\"); } else { return ''; } }" +

                                                @"function getDatabases(name) { CheckAbort(); return window.external.GetDatabases(name); } 

                                                function getTables(name, dbName) { CheckAbort(); return window.external.GetTables(name, dbName); }

                                                function getColumns(name, dbName, table) { CheckAbort(); return window.external.GetColumns(name, dbName, table); }

                                                function getRows(name, dbName, sql) { CheckAbort(); return window.external.GetRows(name, dbName, sql); }

                                                function excuteQuery(name, dbName, sql) { CheckAbort(); return window.external.ExcuteQuery(name, dbName, sql); } 

                                                function removeStopWords(text) { CheckAbort(); return window.external.RemoveStopWords(text); }

                                                function addElement(path, node1, node2, text) { CheckAbort(); return window.external.AddElement(path, node1, node2, text); }

                                                function checkXmlElement(path, node, text) { CheckAbort(); return window.external.CheckXmlElement(path, node, text); }

                                                function getXmlElement(path, node) { CheckAbort(); return window.external.GetXmlElement(path, node); }

                                                function getParentElement(path, node, text) { CheckAbort(); return window.external.GetParentElement(path, node, text); }
                                                
                                                function extractbyRegularExpression(pattern, groupName) { CheckAbort(); return window.external.ExtractUsingRegularExpression(pattern, groupName); }

                                                function addToDownload(fileName, url, folder) { CheckAbort(); return window.external.AddToDownload(fileName, url, folder); }

                                                function startDownload() { CheckAbort(); return window.external.StartDownload(); }

                                                function hide() { CheckAbort(); return window.external.MinimizeWindow(); }
                                            </script>
                                        </head>
                                        <body>
                                            
                                        </body>
                                    </html>";
            //this.Controls.Add(wbMain);
        }

        void ExcuteJSCodeWebBrowser(string code)
        {
            wbMain.Document.InvokeScript("UnAbort");
            object obj = wbMain.Document.InvokeScript("eval", new object[] { code });
        }

        void GoWebBrowser(string url)
        {
            if (String.IsNullOrEmpty(url)) return;
            if (url.Equals("about:blank")) return;

            //GeckoWebBrowser wbBrowser = new GeckoWebBrowser();
            GeckoWebBrowser wbBrowser = CreateGecko();
            wbBrowser.ProgressChanged += wbBrowser_ProgressChanged;
            wbBrowser.Navigated += wbBrowser_Navigated;
            wbBrowser.DocumentCompleted += wbBrowser_DocumentCompleted;
            wbBrowser.CanGoBackChanged += wbBrowser_CanGoBackChanged;
            wbBrowser.CanGoForwardChanged += wbBrowser_CanGoForwardChanged;
            wbBrowser.ShowContextMenu += new EventHandler<GeckoContextMenuEventArgs>(wbBrowser_ShowContextMenu);
            wbBrowser.DomContextMenu += wbBrowser_DomContextMenu;
            wbBrowser.NoDefaultContextMenu = true;

            currentTab.Controls.Add(wbBrowser);
            wbBrowser.Dock = DockStyle.Fill;
            wbBrowser.Navigate(url);
        }

        void wbBrowser_ProgressChanged(object sender, GeckoProgressEventArgs e)
        {
            Application.DoEvents();
        }

        void wbBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
        }

        void wbBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
        }

        void wbBrowser_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            /*
            string url = string.Empty;
            url = ((GeckoWebBrowser)sender).Url.ToString();
            if (url != "about:blank")
                tbxAddress.Text = url;
                */
            e = e;
        }

        void wbBrowser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            if (e.Uri.AbsolutePath != (sender as GeckoWebBrowser).Url.AbsolutePath)
                return;

            GeckoWebBrowser wbBrowser = (GeckoWebBrowser)sender;

            string title = wbBrowser.DocumentTitle;
            currentTab.Text = wbBrowser.Url.ToString();
            

            IsBreakSleep = true;
        }

        void wbBrowser_ShowContextMenu(object sender, GeckoContextMenuEventArgs e)
        {
            //contextMenuBrowser.Show(Cursor.Position);

            //CurrentMouseX = Cursor.Position.X;
            //CurrentMouseY = Cursor.Position.Y;

            /*GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                htmlElm = (GeckoHtmlElement)wb.Document.ElementFromPoint(Cursor.Position.X, Cursor.Position.Y);
                if (htmlElm != null)
                {
                    if (htmlElm.GetType().Name == "GeckoIFrameElement")
                    {
                        var iframe = (GeckoIFrameElement)wb.Document.GetElementById(htmlElm.Id);
                        if (iframe != null)
                        {
                            var contentDocument = iframe.ContentWindow.Document;
                            if (contentDocument != null)
                                htmlElm = (GeckoHtmlElement)contentDocument.ElementFromPoint(Cursor.Position.X, Cursor.Position.Y);
                        }
                    }
                }
            }*/
        }

        private int CurrentMouseX = 0;
        private int CurrentMouseY = 0;

        void wbBrowser_DomContextMenu(object sender, DomMouseEventArgs e)
        {
            if (e.Button.ToString().IndexOf("Right") != -1)
            {
                //contextMenuBrowser.Show(Cursor.Position);

                CurrentMouseX = Cursor.Position.X;
                CurrentMouseY = Cursor.Position.Y;

                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    htmlElm = (GeckoHtmlElement)wb.Document.ElementFromPoint(e.ClientX, e.ClientY);
                }
            }
        }

        void GoWebBrowserByXpath(string xpath)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                GeckoHtmlElement elm = GetCompleteElementByXPath(wb, xpath);
                if (elm != null)
                {
                    UpdateUrlAbsolute(wb.Document, elm);
                    string url = extractData(elm, "href");
                    if (!string.IsNullOrEmpty(url))
                        wb.Navigate(url);
                }
            }
        }

        void NextWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoForward();
            }
        }

        void BackWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoBack();
            }
        }

        void ReloadWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Refresh();
            }
        }

        void StopWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Stop();
            }
        }

        void TabSelectedWebBrowser()
        {
            if (tabMain.TabCount > 0)
            {
                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    //tbxAddress.Text = wb.Url.ToString();
                    string title = wb.DocumentTitle;
                    currentTab.Text = (title.Length > 10 ? title.Substring(0, 10) + "..." : title);
                    this.Text = title;
                }
            }
        }

        private GeckoHtmlElement GetElementByXpath(GeckoDocument doc, string xpath)
        {
            if (doc == null) return null;

            xpath = xpath.Replace("/html/", "");
            GeckoElementCollection eleColec = doc.GetElementsByTagName("html"); if (eleColec.Length == 0) return null;
            GeckoHtmlElement ele = eleColec[0];
            string[] tagList = xpath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tag in tagList)
            {
                System.Text.RegularExpressions.Match mat = System.Text.RegularExpressions.Regex.Match(tag, "(?<tag>.+)\\[@id='(?<id>.+)'\\]");
                if (mat.Success == true)
                {
                    string id = mat.Groups["id"].Value;
                    GeckoHtmlElement tmpEle = doc.GetHtmlElementById(id);
                    if (tmpEle != null) ele = tmpEle;
                    else
                    {
                        ele = null;
                        break;
                    }
                }
                else
                {
                    mat = System.Text.RegularExpressions.Regex.Match(tag, "(?<tag>.+)\\[(?<ind>[0-9]+)\\]");
                    if (mat.Success == false)
                    {
                        GeckoHtmlElement tmpEle = null;
                        foreach (GeckoNode it in ele.ChildNodes)
                        {
                            if (it.NodeName.ToLower() == tag)
                            {
                                tmpEle = (GeckoHtmlElement)it;
                                break;
                            }
                        }
                        if (tmpEle != null) ele = tmpEle;
                        else
                        {
                            ele = null;
                            break;
                        }
                    }
                    else
                    {
                        string tagName = mat.Groups["tag"].Value;
                        int ind = int.Parse(mat.Groups["ind"].Value);
                        int count = 0;
                        GeckoHtmlElement tmpEle = null;
                        foreach (GeckoNode it in ele.ChildNodes)
                        {
                            if (it.NodeName.ToLower() == tagName)
                            {
                                count++;
                                if (ind == count)
                                {
                                    tmpEle = (GeckoHtmlElement)it;
                                    break;
                                }
                            }
                        }
                        if (tmpEle != null) ele = tmpEle;
                        else
                        {
                            ele = null;
                            break;
                        }
                    }
                }
            }

            return ele;
        }

        private string GetXpath(GeckoNode node)
        {
            if (node == null)
                return string.Empty;

            if (node.NodeType == NodeType.Attribute)
            {
                return String.Format("{0}/@{1}", GetXpath(((GeckoAttribute)node).OwnerDocument), node.LocalName);
            }
            if (node.ParentNode == null)
            {
                return "";
            }
            string elementId = ((GeckoHtmlElement)node).Id;
            if (!String.IsNullOrEmpty(elementId))
            {
                return String.Format("//*[@id='{0}']", elementId);
            }

            int indexInParent = 1;
            GeckoNode siblingNode = node.PreviousSibling;

            while (siblingNode != null)
            {

                if (siblingNode.LocalName == node.LocalName)
                {
                    indexInParent++;
                }
                siblingNode = siblingNode.PreviousSibling;
            }

            return String.Format("{0}/{1}[{2}]", GetXpath(node.ParentNode), node.LocalName, indexInParent);
        }

        private int GetXpathIndex(GeckoHtmlElement ele)
        {
            if (ele.Parent == null) return 0;
            int ind = 0, indEle = 0;
            string tagName = ele.TagName;
            GeckoNodeCollection elecol = ele.Parent.ChildNodes;
            foreach (GeckoNode it in elecol)
            {
                if (it.NodeName == tagName)
                {
                    ind++;
                    if (it.TextContent == ele.TextContent) indEle = ind;
                }
            }
            if (ind > 1) return indEle;
            return 0;
        }

        protected void UpdateUrlAbsolute(GeckoDocument doc, GeckoHtmlElement ele)
        {
            string link = doc.Url.GetLeftPart(UriPartial.Authority);

            var eleColec = ele.GetElementsByTagName("IMG");
            foreach (GeckoHtmlElement it in eleColec)
            {
                if (!it.GetAttribute("src").StartsWith("http://"))
                    it.SetAttribute("src", link + it.GetAttribute("src"));
            }
            eleColec = ele.GetElementsByTagName("A");
            foreach (GeckoHtmlElement it in eleColec)
            {
                if (!it.GetAttribute("href").StartsWith("http://"))
                    it.SetAttribute("href", link + it.GetAttribute("href"));
            }
        }

        private GeckoHtmlElement GetCompleteElementByXPath(GeckoWebBrowser wb, string xpath)
        {
            GeckoHtmlElement elm = GetElement(wb, xpath);

            int waitUntil = 0;
            int count = 0;

            int.TryParse(MaxWait, out waitUntil);

            while (elm == null)
            {
                //Stop when click Stop button
                if (IsStop) break;

                //It will stop when get the limit configuration
                if (count > waitUntil) break;

                elm = GetElement(wb, xpath);
                sleep(1, false);
                count++;
            }

            return elm;
        }

        private GeckoHtmlElement GetElement(GeckoWebBrowser wb, string xpath)
        {
            GeckoHtmlElement elm = null;
            if (xpath.StartsWith("/"))
            {
                if (xpath.Contains("@class") || xpath.Contains("@data-type"))
                {
                    var html = GetHtmlFromGeckoDocument(wb.Document);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    var node = doc.DocumentNode.SelectSingleNode(xpath);
                    if (node != null)
                    {
                        var currentXpath = "/" + node.XPath;
                        elm = (GeckoHtmlElement)wb.Document.EvaluateXPath(currentXpath).GetNodes().FirstOrDefault();
                    }
                }
                else
                {
                    elm = (GeckoHtmlElement)wb.Document.EvaluateXPath(xpath).GetNodes().FirstOrDefault();
                }
            }
            else
            {
                elm = (GeckoHtmlElement)wb.Document.GetElementById(xpath);
            }
            return elm;
        }

        private string GetHtmlFromGeckoDocument(GeckoDocument doc)
        {
            var result = string.Empty;

            GeckoHtmlElement element = null;
            var geckoDomElement = doc.DocumentElement;
            if (geckoDomElement is GeckoHtmlElement)
            {
                element = (GeckoHtmlElement)geckoDomElement;
                result = element.InnerHtml;
            }

            return result;
        }

        #endregion  

        #region functions
        public string extract(string xpath, string type)
        {
            string result = string.Empty;
            GeckoHtmlElement elm = null;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                elm = GetElement(wb, xpath);
                if (elm != null)
                    UpdateUrlAbsolute(wb.Document, elm);

                if (elm != null)
                {
                    switch (type)
                    {
                        case "html":
                            result = elm.OuterHtml;
                            break;
                        case "text":
                            if (elm.GetType().Name == "GeckoTextAreaElement")
                            {
                                result = ((GeckoTextAreaElement)elm).Value;
                            }
                            else
                            {
                                result = elm.TextContent.Trim();
                            }
                            break;
                        case "value":
                            result = ((GeckoInputElement)elm).Value;
                            break;
                        default:
                            result = extractData(elm, type);
                            break;
                    }
                }
            }

            return result;
        }

        public string extractUntil(string xpath, string type)
        {
            var result = string.Empty;

            GeckoHtmlElement elm = null;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                elm = GetCompleteElementByXPath(wb, xpath);
                if (elm != null)
                    UpdateUrlAbsolute(wb.Document, elm);

                if (elm != null)
                {
                    switch (type)
                    {
                        case "html":
                            result = elm.OuterHtml;
                            break;
                        case "text":
                            if (elm.GetType().Name == "GeckoTextAreaElement")
                            {
                                result = ((GeckoTextAreaElement)elm).Value;
                            }
                            else
                            {
                                result = elm.TextContent.Trim();
                            }
                            break;
                        default:
                            result = extractData(elm, type);
                            break;
                    }
                }
            }

            return result;
        }

        public void filliframe(string title, string value)
        {
            /*GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                foreach (GeckoWindow ifr in wb.Window.Frames)
                {
                    if (ifr.Document.Title == title)
                    {
                        foreach (var item in ifr.Document.ChildNodes)
                        {
                            if (item.NodeName == "HTML")
                            {
                                foreach (var it in item.ChildNodes)
                                {
                                    if (it.NodeName == "BODY")
                                    {
                                        GeckoBodyElement elem = (GeckoBodyElement)it;
                                        elem.InnerHtml = value;
                                        elem.Focus();
                                    }
                                }                                
                                break;
                            }
                        }
                        break;
                    }
                }
            }*/
        }

        public void fill(string xpath, string value)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        switch (elm.TagName)
                        {
                            case "IFRAME":
                                /*foreach (GeckoWindow ifr in wb.Window.Frames)
                                {
                                    if (ifr.Document == elm.DOMElement)
                                    {
                                        ifr.Document.TextContent = value;
                                        break;
                                    }
                                }*/
                                break;
                            case "INPUT":
                                GeckoInputElement input = (GeckoInputElement)elm;
                                input.Value = value;
                                input.Focus();
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Byte[] bytes = Encoding.UTF32.GetBytes(value);
                    StringBuilder asAscii = new StringBuilder();
                    for (int idx = 0; idx < bytes.Length; idx += 4)
                    {
                        uint codepoint = BitConverter.ToUInt32(bytes, idx);
                        if (codepoint <= 127)
                            asAscii.Append(Convert.ToChar(codepoint));
                        else
                            asAscii.AppendFormat("\\u{0:x4}", codepoint);
                    }
                    /*var id = xpath;
                    using (AutoJSContext context = new AutoJSContext(wb.Window.JSContext))
                    {
                        context.EvaluateScript("document.getElementById('" + id + "').value = '" + asAscii.ToString() + "';");
                        context.EvaluateScript("document.getElementById('" + id + "').scrollIntoView();");
                    }*/
                }
            }

        }

        public void filldropdown(string xpath, string value)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        var dropdown = elm as GeckoSelectElement;
                        var length = dropdown.Options.Length;
                        var items = dropdown.Options;
                        for (var i = 0; i < length; i++)
                        {
                            var item = dropdown.Options.item((uint)i);
                            if (item.Text.ToUpper() == value.ToUpper())
                            {
                                item.SetAttribute("selected", "selected");
                            }
                            else
                            {
                                item.RemoveAttribute("selected");
                            }
                        }
                        elm.Focus();
                    }
                }
                else
                {
                }
            }
        }

        public void click(string xpath)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        elm.Click();
                        elm.Focus();
                    }
                }
                else
                {
                }
            }
        }

        public void sleep(double seconds, bool isBreakWhenWBCompleted)
        {
            IsBreakSleep = false;
            for (int i = 0; i < seconds * 10; i++)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);

                if (isBreakWhenWBCompleted && IsBreakSleep)
                {
                    break;
                }
            }
        }
        public string read(string path)
        {
            string result = "";
            try
            {
                string[] list = System.IO.File.ReadAllLines(path);
                foreach (string l in list)
                {
                    result += l + "\n";
                }
            }
            catch { }
            return result;
        }
        public void runcommand(string path, string parameters)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = getCurrentPath();
                startInfo.FileName = path;
                startInfo.Arguments = parameters;

                try
                {
                    Process p = Process.Start(startInfo);
                    p.WaitForExit();
                }
                catch { }
            }
            catch { }
        }

        public void save(string content, string path, bool isOverride)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, isOverride))
                {
                    file.WriteLine(content);
                }
            }
            catch { }
        }
        public void excute(string script)
        {
            ExcuteJSCodeWebBrowser(script);
        }
        public string encrypt(string publicKey, string plainText)
        {
            System.Security.Cryptography.CspParameters cspParams = null;
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = null;
            byte[] plainBytes = null;
            byte[] encryptedBytes = null;

            string result = "";
            try
            {
                cspParams = new System.Security.Cryptography.CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(publicKey);

                plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);
                result = Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex) { }
            return result;
        }

        public string decrypt(string privateKey, string encrypted)
        {
            System.Security.Cryptography.CspParameters cspParams = null;
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = null;
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            string result = "";
            try
            {
                cspParams = new System.Security.Cryptography.CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(privateKey);

                encryptedBytes = Convert.FromBase64String(encrypted);
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                result = System.Text.Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex) { }
            return result;
        }
        public void TakeSnapshot(string location)
        {
            try
            {
                GeckoWebBrowser wbBrowser = (GeckoWebBrowser)GetCurrentWB();
                ImageCreator creator = new ImageCreator(wbBrowser);
                byte[] rs = creator.CanvasGetPngImage((uint)wbBrowser.Document.ActiveElement.ScrollWidth, (uint)wbBrowser.Document.ActiveElement.ScrollHeight);


                MemoryStream ms = new MemoryStream(rs);
                Image returnImage = Image.FromStream(ms);

                returnImage.Save(location);

            }
            catch { }
        }

        public string imgToText(string xpath, string language)
        {
            string data = string.Empty;
            string path = string.Empty;
            path = Application.StartupPath + "\\captcha\\image.png";
            bool isSaveSuccess = saveImage(xpath, path);

            if (isSaveSuccess)
            {
                string text = Application.StartupPath + "\\captcha\\output.txt";

                string param = "";
                if (language == "vie")
                {
                    param = "\"" + path + "\" \"" + Application.StartupPath + "\\captcha\\output" + "\" -l vie";
                }
                else
                {
                    param = "\"" + path + "\" \"" + Application.StartupPath + "\\captcha\\output" + "\" -l eng";
                }


                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Application.StartupPath + "\\tesseract.exe";
                process.StartInfo.Arguments = param;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                data = read(text).Replace("\n", "");

            }

            return data;
        }

        private bool saveImage(string xpath, string location)
        {
            bool result = false;
            try
            {
                GeckoWebBrowser wbBrowser = (GeckoWebBrowser)GetCurrentWB();
                if (wbBrowser != null)
                {
                    GeckoImageElement element = null;
                    if (xpath.StartsWith("/"))
                        element = (GeckoImageElement)wbBrowser.Document.EvaluateXPath(xpath).GetNodes().FirstOrDefault();
                    else
                        element = (GeckoImageElement)wbBrowser.Document.GetElementById(xpath);
                    GeckoSelection selection = wbBrowser.Window.Selection;
                    selection.SelectAllChildren(element);
                    wbBrowser.CopyImageContents();
                    if (Clipboard.ContainsImage())
                    {
                        Image img = Clipboard.GetImage();
                        img.Save(location, System.Drawing.Imaging.ImageFormat.Png);
                        result = true;
                    }
                }
            }
            catch { result = false; }

            return result;
        }
        public void explorer(string path)
        {
            string argument = "/select, \"" + path + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
        public void loadHTML(string path)
        {
            go(path);
        }

        public string textToJSON(string text)
        {
            return text;
        }

        public string GetAccountBy(string name)
        {
            string result = "";

            if (User.Current != null)
            {
                Account account = new Account();
                result = account.GetByJSON(name);
            }
            return result;
        }

        public string sendEmail(string name, string email, string subject, string content)
        {
            string result = "False";

            if (User.Current != null && !string.IsNullOrEmpty(name))
            {
                MailServer mailserver = new MailServer();
                var item = mailserver.GetBy(name);
                if (item != null)
                {
                    mailserver.Username = item.Username;
                    mailserver.Password = item.Password;
                    mailserver.Server = item.Server;
                    mailserver.Port = item.Port;

                    result = mailserver.SendEmail(email, subject, content).ToString();
                }
            }
            return result;
        }

        public void CaptchaBorder(string xpath, string style)
        {

        }

        [ComImport, InterfaceType((short)1), Guid("3050F669-98B5-11CF-BB82-00AA00BDCE0B")]
        private interface IHTMLElementRenderFixed
        {
            void DrawToDC(IntPtr hdc);
            void SetDocumentPrinter(string bstrPrinterName, IntPtr hdc);
        }

        public void SaveImageFromElement(string xpath, string path)
        {
            saveImage(xpath, path);
        }
        public void Abort()
        {
            IsStop = true;
            Stop();
        }

        public void UnAbort()
        {
            IsStop = false;
        }
        public void Stop()
        {
        }
        public string GetDatabases(string name)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetDatabases(name);

            return result;
        }

        public string GetTables(string name, string dbName)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetTables(name, dbName);

            return result;
        }

        public string GetColumns(string name, string dbName, string table)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetColumns(name, dbName, table);

            return result;
        }

        public string GetRows(string name, string dbName, string sql)
        {
            string result = "";

            Connection conn = new Connection();

            result = conn.GetRows(name, dbName, sql);

            return result;
        }

        public int ExcuteQuery(string name, string dbName, string sql)
        {
            int result = -1;

            Connection conn = new Connection();
            try
            {
                result = conn.ExcuteQuery(name, dbName, sql);
            }
            catch { }
            return result;
        }
        public void AddElement(string path, string node1, string node2, string text)
        {
            XDocument xdoc = XDocument.Load(path);
            var item = xdoc.Element(node1).Element(node2);
            if (item != null)
                item.Add(new XElement("w", text));
            else
                xdoc.Element(node1).Add(new XElement(node2, new XElement("w", text)));

            xdoc.Save(path);
        }

        public bool CheckXmlElement(string path, string node, string text)
        {
            bool result = false;

            XDocument xdoc = XDocument.Load(path);

            var rs = (from w in xdoc.Descendants(node) where w.Value == text select w).FirstOrDefault();
            if (rs != null) result = true;

            return result;
        }

        public string GetParentElement(string path, string node, string text)
        {
            string result = string.Empty;

            XDocument xdoc = XDocument.Load(path);

            var rs = (from w in xdoc.Descendants(node) where w.Value == text select w).FirstOrDefault();
            if (rs != null) result = rs.Parent.Name.LocalName;

            return result;
        }

        public string GetXmlElement(string path, string node)
        {
            string result = string.Empty;
            List<string> data = new List<string>();
            XDocument xdoc = XDocument.Load(path);
            var list = xdoc.Root.Nodes();
            foreach (XElement elem in list)
            {
                data.Add(elem.Name.LocalName);
            }
            result = string.Join(",", data);
            return result;
        }

        public string ExtractUsingRegularExpression(string pattern, string groupName)
        {
            string result = string.Empty;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                string doc = wb.Document.Body.TextContent;
                Match m = Regex.Match(doc, pattern);
                if (m.Success)
                {
                    if (m.Groups.Count > 0)
                    {
                        result = m.Groups[groupName].Value;
                    }
                }
            }

            return result;
        }


        private string extractData(GeckoHtmlElement ele, string attribute)
        {
            var result = string.Empty;

            if (ele != null)
            {
                var tmp = ele.GetAttribute(attribute);
                /*if (tmp == null)
                {
                    tmp = extractData(ele.Parent, attribute);
                }*/
                if (tmp != null)
                    result = tmp.Trim();
            }

            return result;
        }
        public string getCurrentPath()
        {
            string result = "";
            try
            {
                result = Application.StartupPath;
            }
            catch { }
            return result;
        }
        #endregion
        #region Excel

        public string readCellExcel(string filePath, string isheetname, int irow, int icolumn)
        {
            string result = "";
            try
            {
                using (StreamReader input = new StreamReader(filePath))
                {
                    NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(new NPOI.POIFS.FileSystem.POIFSFileSystem(input.BaseStream));
                    if (null == workbook)
                    {
                        result = "";
                    }

                    NPOI.HSSF.UserModel.HSSFFormulaEvaluator formulaEvaluator = new NPOI.HSSF.UserModel.HSSFFormulaEvaluator(workbook);
                    NPOI.HSSF.UserModel.HSSFDataFormatter dataFormatter = new NPOI.HSSF.UserModel.HSSFDataFormatter(new CultureInfo("vi-VN"));

                    NPOI.SS.UserModel.ISheet sheet = workbook.GetSheet(isheetname);
                    NPOI.SS.UserModel.IRow row = sheet.GetRow(irow);

                    if (row != null)
                    {
                        short minColIndex = row.FirstCellNum;
                        short maxColIndex = row.LastCellNum;

                        if (icolumn >= minColIndex || icolumn <= maxColIndex)
                        {
                            NPOI.SS.UserModel.ICell cell = row.GetCell(icolumn);
                            if (cell != null)
                            {
                                if (cell.CellType == NPOI.SS.UserModel.CellType.FORMULA)
                                {
                                    result = cell.StringCellValue;
                                }
                                else
                                {
                                    result = dataFormatter.FormatCellValue(cell, formulaEvaluator);
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            return result;
        }

        public void writeCellExcel(string filePath, string sheetname, string cellName, string value)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workbook;
            if (!File.Exists(filePath))
            {
                workbook = NPOI.HSSF.UserModel.HSSFWorkbook.Create(NPOI.HSSF.Model.InternalWorkbook.CreateWorkbook());
                var sheet = workbook.CreateSheet(sheetname);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
            using (var input = new StreamReader(filePath))
            {
                workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(new NPOI.POIFS.FileSystem.POIFSFileSystem(input.BaseStream));
                if (workbook != null)
                {
                    var sheet = workbook.GetSheet(sheetname);
                    NPOI.SS.Util.CellReference celRef = new NPOI.SS.Util.CellReference(cellName);
                    var row = sheet.GetRow(celRef.Row);
                    if (row == null)
                        row = sheet.CreateRow(celRef.Row);

                    var cell = row.GetCell(celRef.Col);
                    if (cell == null)
                        cell = row.CreateCell(celRef.Col);

                    cell.SetCellValue(value);
                }
            }

            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                workbook.Write(file);
                file.Close();
            }
        }

        #endregion

        delegate void VoidArgDelegate();
        delegate void LoginDelegate(string email, string password);
        private void BeginLoading()
        {
            if(resultList.InvokeRequired)
            {
                VoidArgDelegate d = new VoidArgDelegate(BeginLoading);
                this.Invoke(d);
            }
            else
            {
                resultList.Items.Clear();
                resultList.Enabled = false;
            }
        }

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        private string SelectRandomTextInList(StringList stringList)
        {
            int index = rnd.Next(0, stringList.Count);

            return stringList[index];
        }
        
        private void TypingThreadProc(IntPtr hWnd, string text)
        {
            //MessageBox.Show(Thread.CurrentThread.Name);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if(c == '\n')
                {
                    SendMessage((IntPtr)hWnd, WM_KEYDOWN, (IntPtr)VK_RETURN, 1);
                    SendMessage((IntPtr)hWnd, WM_KEYUP, (IntPtr)VK_RETURN, 1);
                    continue;
                }
                IntPtr val = new IntPtr((Int32)c);
                SendMessage((IntPtr)hWnd, WM_CHAR, val, 1);
                Application.DoEvents();
                Thread.Sleep(30);
            }
        }

        private void StartTypingThread(IntPtr hWnd, string text)
        {
            /*ThreadStart starter = delegate { TypingThreadProc(hWnd, text); };
            var thread = new Thread(starter);
            thread.IsBackground = true;
            thread.Start();*/
            TypingThreadProc(hWnd, text);
        }

        private string GetUpperPath(string xPath, int nGoUpperTimes = 1)
        {
            string res = xPath;

            for(int i = 0; i < nGoUpperTimes; i++)
            {
                int pos = res.LastIndexOf("/");

                if (pos == -1)
                {
                    res = "";
                    break;
                }
                else
                    res = res.Substring(0, pos);
            }

            return res;
        }
        
        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int WM_CHAR = 0x102;
        const int VK_RETURN = 0x0D;
        const int VK_END = 0x23;
        const int VK_PRIOR = 0x21; // Page Up
        const int VK_NEXT = 0x22;  // Page Down
        const int VK_UP = 0x26;
        const int VK_DOWN = 0x28;

        const int INVALID_URL = -3;
        const int LOGIN_ERROR = -2;
        const int LOGIN_INVALID = -1;
        const int LOGIN_FAIL = 0;
        const int LOGIN_SUCCESS = 1;
        const int LOGIN_PIN = 2;

        const int NEW_URL = 1;
        const int OLD_URL = 2;
        const int NEW_REG_URL = 3;
        private void Type(string text, IntPtr handle)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                IntPtr val = new IntPtr((Int32)c);
                SendMessage((IntPtr)handle, WM_KEYDOWN, val, 1);
                SendMessage((IntPtr)handle, WM_CHAR, val, 1);
                SendMessage((IntPtr)handle, WM_KEYUP, val, 1);
            }
        }
        private void PressEnter(IntPtr handle)
        {
            SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_RETURN, 1);
            Thread.Sleep(100);
            SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_RETURN, 1);
            Thread.Sleep(100);

            SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_RETURN, 1);
            Thread.Sleep(100);
            SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_RETURN, 1);
            Thread.Sleep(100);

            SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_RETURN, 1);
            Thread.Sleep(100);
            SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_RETURN, 1);
            Thread.Sleep(100);
        }
        private void PressArrowUpDown(IntPtr handle, int nrep = 1)
        {
            for (int i = 0; i < Math.Abs(nrep); i++)
            {
                if (nrep >= 0)
                {
                    SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_DOWN, 1);
                    Thread.Sleep(50);
                    SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_DOWN, 1);
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
                else
                {
                    SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_UP, 1);
                    Thread.Sleep(50);
                    SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_UP, 1);
                    Thread.Sleep(50);
                }

            }

        }
        private void PressPageUpDown(IntPtr handle, int nrep = 1)
        {
            for (int i = 0; i < Math.Abs(nrep); i++)
            {
                if (nrep >= 0)
                {
                    SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_NEXT, 1);
                    Thread.Sleep(50);
                    SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_NEXT, 1);
                    Application.DoEvents();
                    Thread.Sleep(50);

                }
                else
                {
                    SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_PRIOR, 1);
                    Thread.Sleep(50);
                    SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_PRIOR, 1);
                    Application.DoEvents();
                    Thread.Sleep(50);
                }

            }
        }
        #region Control Message Handlers
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process targetProcesses = Process.GetCurrentProcess();
            targetProcesses.Kill();
            IsRunning = false;
        }

        #endregion

        #region Automation system
        private string logfile = "bot.log";
        private void btnComStartStop_Click(object sender, EventArgs e)
        {

            if (IsRunning)
            {
                btnComStartStop.Enabled = false;
                IsRunning = false;
            }
            else
            {
                //==========================   License Start  ==========================
                bool res = LicenseUtil.CountExpired();
                if (res == true)
                {
                    MessageBox.Show("Hi, client! \nThe website was updated with some additional operations." + "\nTherefore, contact your vendor to purchase full-patched version of this application.", "Notification - Index");
                    Application.Exit();
                    return;

                }
                bool fe = LicenseUtil.IsExpiredDate();
                if (fe) {
                    MessageBox.Show("Hi, client! \nThe website was updated with some additional operations." + "\nTherefore, contact your vendor to purchase full-patched version of this application.", "Notification - Date");
                    Application.Exit();
                    return;
                }
                //====================    License End ==========================
                IsRunning = true;
                btnComStartStop.Text = "Stop";
                StartRegister();
            }
           
        }
        static int k = 0;
        private void StartRegister()
        {

            progBar.Value = 0;
            percentLabel.Visible = true;
            statusLabel.Text = "Checking....";
            resultList.Items.Clear();
            radLogin.Enabled = false;
            radRegister.Enabled = false;


            string good = "Registered", bad = "Unregistered";
            int accout_cnt = tblAccounts.Rows.Count - 1;
            if (accout_cnt <= 0)
            {
                MessageBox.Show("Current account is invalid.", "Account Error");
                return;
            }
            // Checking Existing Files
            if (File.Exists(Application.StartupPath + "\\export\\" + good + ".csv")
                || File.Exists(Application.StartupPath + "\\export\\" + bad + ".csv")
             )
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to append new results into existing files?\n" +
                    "Recommended to back up existing files!", "Confirmation", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    File.Create(Application.StartupPath + "\\export\\" + good + ".csv").Close();
                    File.Create(Application.StartupPath + "\\export\\" + bad + ".csv").Close();
                    
                }
            }
            int start = Int32.Parse(txtFrom.Text);
            int end = Int32.Parse(txtTo.Text);
            if (start <= 0) start = 1;
            if (end > accout_cnt) end = accout_cnt;
            if (start > end) return;

            //Inspecting accounts
            string cc = "https://reg.ebay.com/reg/PartialReg";

            for (int i = start - 1; i < end && IsRunning; i++)
            {

                Application.DoEvents();
                string strEmail = (String)tblAccounts.Rows[i].Cells[0].Value;
                string strPass = (String)tblAccounts.Rows[i].Cells[1].Value;
                string ua;
                ua = tblUA.Rows.Count - 1 > 0 ? tblUA.Rows[k % (tblUA.Rows.Count - 1)].Cells[0].Value.ToString() : "";
                setUA(ua);
                // Login
                int res = DoRegister(strEmail, strPass, cc);
                string[] row_t = new string[5];
                row_t[0] = (i + 1).ToString();
                row_t[1] = strEmail;
                k++;
                if (res == LOGIN_SUCCESS)
                {
                    row_t[2] = good;
                }
               
                else if (res == LOGIN_FAIL)
                {
                    row_t[2] = bad;
                }
                else if (res == LOGIN_INVALID)
                {
                    row_t[2] = bad;
                }
                else
                {
                    //if (res == LOGIN_ERROR)
                        //SessionTimeout(Int32.Parse(txtTimeout.Text));
                    i = i - 1;
                    //ua = tblUA.Rows.Count - 1 > 0 ? tblUA.Rows[k % (tblUA.Rows.Count - 1)].Cells[0].Value.ToString() : "";
                    //setUA(ua);
                    //RemoveAllCache();
                    goRegister(cc);
                    continue;
                }
                var used_ua = Gecko.GeckoPreferences.User["general.useragent.override"];
                row_t[3] = cc;
                row_t[4] = used_ua != null ? used_ua.ToString() : "Default User Agent";


                string newline = string.Format("{0},{1}" + Environment.NewLine, strEmail, strPass);
                File.AppendAllText(Application.StartupPath + "\\export\\" + row_t[2] + ".csv", newline);

                ListViewItem viewItem = new ListViewItem(row_t);
                resultList.Items.Add(viewItem);

                // Processing progress bar and status
                int percent = (i + 1) * 100 / accout_cnt;
                percentLabel.Text = string.Format("{0}%", percent);
                progBar.Value = percent;
                RemoveAllCache();
//                goRegister(cc);
                //////////////// Counting License///////////////////

                if (LicenseUtil.CountExpired() == true)
                {
                    MessageBox.Show("Hi, client! \nThe website was updated with some additional operations." + "\nTherefore, contact your vendor to purchase patched version of this application.", "Notification - Index - Processing");
                    Application.Exit();
                    return;
                }

            }
            btnComStartStop.Enabled = true;
            btnComStartStop.Text = "Start";
            IsRunning = false;
            statusLabel.Text = "Ready";
            percentLabel.Visible = false;
            radLogin.Enabled = true;
            radRegister.Enabled = true;

        }
        private void DoLogout(string cc = "LV")
        {
            try
            {
                click("//*[@id='moreMenuDesktopProfileLink']/div[1]/a[1]");
            }
            catch
            {
            }
            Logging("Loggin out.....");
            sleep(1, true);
            RemoveAllCache();
        }

        private bool CheckPass(string pw)
        {
            return pw.All(c => Char.IsLetter(c) && Char.IsDigit(c)) && pw.Length >=8;
        }
        private int DoRegister(string em, string pwd, string surl)
        {
            if(! CheckPass(pwd))
                pwd = "Abc123#@!";
            if (String.IsNullOrEmpty(em))
                return LOGIN_INVALID;

            GeckoWebBrowser wb; string curUrl;
            IntPtr child1handle, child2handle;
            wb = (GeckoWebBrowser)GetCurrentWB();
            curUrl = wb.Url.AbsoluteUri;

            if (CheckURL(curUrl) != true)
            {
                RemoveAllCache();
                return INVALID_URL;
            }
            while(true)
            {
                //Loading
                sleep(2, true);
                string html = extract("//*[@id='email_w']", "text");
                if (String.IsNullOrEmpty(html) == false) break;
            }
            Logging("Ready for checking.....");
            //sleep(100, true);
            try
            {
                /*
                    text = extract("//*[@id='email_w']", "text");
                    fill("//*[@id='email']", "test@test.com");
                    fill("//*[@id='firstname']", "1");
                    click("//*[@id='firstname']");
                */


                wb.Focus();
                child1handle = FindWindowEx(wb.Handle, IntPtr.Zero, "MozillaWindowClass", "");
                child2handle = FindWindowEx(child1handle, IntPtr.Zero, "MozillaWindowClass", "");

                //Focus on Email Address
                fill("//*[@id='email']", "");
                GeckoHtmlElement commentTextArea = (GeckoHtmlElement)wb.Document.EvaluateXPath("//*[@id='email']").GetSingleNodeValue();
                commentTextArea.Focus();
                commentTextArea.ScrollIntoView(false);
                StartTypingThread(child2handle, em);
                sleep(0.1, true);
                //Focus on Firstname
                fill("//*[@id='firstname']", "");
                click("//*[@id='firstname']");
                sleep(1, true);
            }
            catch
            {
                return LOGIN_ERROR;
            }
            //The first fail checking
            Logging("----- Validating: " + em);

            string regresult = "";
            int cnt = 0;
            string email = extract("//*[@id='email']", "html");
            int limit = 3;

            while (true)
            {
                cnt++;
                sleep(1, true);
                Logging("-----" + $" [{cnt}] " + "Checking notification and validation...");
                regresult = extract("//*[@id='email_w']", "text");

                if (regresult.IndexOf("already") > 0)
                    return LOGIN_SUCCESS;
                else if (regresult.IndexOf("wrong") > 0)
                    return LOGIN_INVALID;
                if (String.IsNullOrEmpty(regresult))
                    return LOGIN_ERROR;
                if(String.IsNullOrEmpty(email))
                    return LOGIN_ERROR;
                if (regresult.IndexOf("enter") > 0 && cnt > limit)
                    return LOGIN_FAIL;
            }
        }


        private void RemoveAllCache()
        {
            //nsICookieManager CookieMan;
            //CookieMan = Xpcom.GetService<nsICookieManager>("@mozilla.org/cookiemanager;1");
            //CookieMan = Xpcom.QueryInterface<nsICookieManager>(CookieMan);
            //CookieMan.RemoveAll();
            //GeckoWebBrowser geckobrowser;
            //geckobrowser = (GeckoWebBrowser)GetCurrentWB();
            //            geckobrowser.Dispose();
            //            Gecko.Xpcom.Shutdown();

            //            foreach (string file in Directory.GetFiles(Gecko.Xpcom.ProfileDirectory))
            //                File.Delete(file);

            //           CallBackWinAppWebBrowser();
            //var path = Application.StartupPath + "\\Firefox";
            //Xpcom.Initialize(path);
        }
    private void goRegister(string surl)
        {
            go(surl);
            sleep(30, true);
        }
        private void Logging(string lg = null)
        {
            if (String.IsNullOrEmpty(lg)) return;
            lstLog.Items.Add(lg + Environment.NewLine);
            File.AppendAllText(Application.StartupPath + "\\export\\" + logfile, lg+ Environment.NewLine);
        }
        #endregion
        #region Event Handlers
        private void tblAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == 1) && e.Value != null)
            {
                tblAccounts.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('\u25CF', e.Value.ToString().Length);
            }
        }

        private void tblAccounts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (tblAccounts.CurrentCell.ColumnIndex == 1)//select target column
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.UseSystemPasswordChar = true;
                }
            }
            else
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.UseSystemPasswordChar = false;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl2.SelectedIndex = (sender as TabControl).SelectedIndex;
        }

        private void btnLoad1_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Excel csv Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            string fname = dlg.FileName;
            DataTable accounts = MGBot.DataImport.NewDataTable(fname, false);
            tblAccounts.Rows.Clear();
            foreach (DataRow row in accounts.Rows)
            {
                string name = row[0].ToString().Trim();
                string password = row[1].ToString().Trim();

                DataGridViewRow gridrow = (DataGridViewRow)tblAccounts.Rows[0].Clone();
                gridrow.Cells[0].Value = name;
                gridrow.Cells[1].Value = password;
                tblAccounts.Rows.Add(gridrow);
            }
            txtTo.Text = accounts.Rows.Count.ToString();
        }

        private void tblAccounts_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridView gridView = sender as DataGridView;
            if (null != gridView)
            {
                foreach (DataGridViewRow r in gridView.Rows)
                {
                    gridView.Rows[r.Index].HeaderCell.Value = (r.Index + 1).ToString();
                }
            }
        }
        #endregion
        #region Initialize Methods
        private void Load_AccountList()
        {
            //Loading Data ... ...
            try
            {
                DataTable acclist = MGBot.DataImport.NewDataTable(Application.StartupPath + "\\import\\ac.txt", false, 2);
                tblAccounts.Rows.Clear();
                foreach (DataRow row in acclist.Rows)
                {
                    string em = row[0].ToString().Trim();
                    string pw = row[1].ToString().Trim();
                    DataGridViewRow gridrow = (DataGridViewRow)tblAccounts.Rows[0].Clone();
                    gridrow.Cells[0].Value = em;
                    gridrow.Cells[1].Value = pw;
                    tblAccounts.Rows.Add(gridrow);
                }
                txtTo.Text = acclist.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void Load_UAlist()
        {
            try
            {
                DataTable uslist = MGBot.DataImport.NewDataTable(Application.StartupPath + "\\import\\ua.txt", false, 1);
                tblUA.Rows.Clear();
                foreach (DataRow row in uslist.Rows)
                {
                    string name = row[0].ToString().Trim();
                    DataGridViewRow gridrow = (DataGridViewRow)tblUA.Rows[0].Clone();
                    gridrow.Cells[0].Value = name;
                    tblUA.Rows.Add(gridrow);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private bool CheckURL(string surl)
        {
            if (surl.IndexOf("PartialReg") > 0)
                return true;
            return false;
        }

        private void setUA(string ua)
        {
            if (!String.IsNullOrEmpty(ua))
              Gecko.GeckoPreferences.User["general.useragent.override"] = ua;
        }
        /// <summary>
        ///     Syncs the cert_override.txt from FireFox, Mozilla, etc.
        /// </summary>
        private void SyncCerts()
        {
            string geckodir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                           "Geckofx\\DefaultProfile\\");
            string gecko = geckodir + "cert_override.txt";

            string geckoContent = File.Exists(gecko) ? File.ReadAllText(gecko) : null;

            // Get every cert_override.txt file for every Mozilla app
            string mozilla = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                          "Mozilla");
            string result = "";
            foreach (string file in this.FindFiles(mozilla, "cert_override.txt"))
            {
                result += Environment.NewLine;
                StreamReader stream = new StreamReader(file);
                while (stream.Peek() >= 0)
                {
                    string line = stream.ReadLine();
                    if (line.StartsWith("#"))
                        continue;

                    if (string.IsNullOrEmpty(geckoContent) || !geckoContent.Contains(line))
                    {
                        result += Environment.NewLine + line;
                    }
                }
            }

            if (!string.IsNullOrEmpty(result.Trim()))
            {
                try
                {
                    if (!Directory.Exists(geckodir))
                        Directory.CreateDirectory(geckodir);
                }
                catch
                {
                }

                try
                {
                    File.WriteAllText(gecko, result, Encoding.UTF8);
                }
                catch
                {
                }
            }
        }
        private GeckoWebBrowser CreateGecko()
        {
            this.SyncCerts();

            var path = Application.StartupPath + "\\Firefox";
            Xpcom.Initialize(path);

            PromptFactory.PromptServiceCreator = () => new FilteredPromptService();
            // Set some preferences
            nsIPrefBranch pref = Xpcom.GetService<nsIPrefBranch>("@mozilla.org/preferences-service;1");

            // Show same page as firefox does for unsecure SSL/TLS connections ...
            pref.SetIntPref("browser.ssl_override_behavior", 1);
            pref.SetIntPref("security.OCSP.enabled", 0);
            pref.SetBoolPref("security.OCSP.require", false);
            pref.SetBoolPref("extensions.hotfix.cert.checkAttributes", true);
            pref.SetBoolPref("security.remember_cert_checkbox_default_setting", true);
            pref.SetBoolPref("services.sync.prefs.sync.security.default_personal_cert", true);
            pref.SetBoolPref("browser.xul.error_pages.enabled", true);
            pref.SetBoolPref("browser.xul.error_pages.expert_bad_cert", false);

            // disable caching of http documents
            pref.SetBoolPref("network.http.use-cache", false);

            // disalbe memory caching
            pref.SetBoolPref("browser.cache.memory.enable", false);

            pref.SetBoolPref("browser.cache.disk.enable", false);
            pref.SetBoolPref("browser.cache.disk_cache_ssl", false);
            pref.SetIntPref("browser.cache.frecency_half_life_hours", 0);
            pref.SetIntPref("browser.cache.disk.capacity", 0);
            pref.SetIntPref("browser.cache.max_shutdown_io_lag", 0);
            pref.SetIntPref("network.cookie.cookieBehavior", 3);
            pref.SetIntPref("network.cookie.lifetimePolicy", 2);
            pref.SetIntPref("network.cookie.lifetime.days", 0);
            pref.SetBoolPref("network.cookie.thirdparty.sessionOnly", false);
            pref.SetIntPref("plugin.sessionPermissionNow.intervalInMinutes", 0);
            pref.SetIntPref("browser.sessionhistory.max_total_viewers", 0);



            // Desktop Notification
            pref.SetBoolPref("notification.feature.enabled", false);

            // WebSMS
            pref.SetBoolPref("dom.sms.enabled", false);
            pref.SetCharPref("dom.sms.whitelist", "");

            // WebContacts
            pref.SetBoolPref("dom.mozContacts.enabled", true);
            pref.SetCharPref("dom.mozContacts.whitelist", "");

            pref.SetBoolPref("social.enabled", false);

            // WebAlarms
            pref.SetBoolPref("dom.mozAlarms.enabled", false);

            // WebSettings
            pref.SetBoolPref("dom.mozSettings.enabled", true);

            pref.SetBoolPref("network.jar.open-unsafe-types", true);
            pref.SetBoolPref("security.warn_entering_secure", false);
            pref.SetBoolPref("security.warn_entering_weak", false);
            pref.SetBoolPref("security.warn_leaving_secure", false);
            pref.SetBoolPref("security.warn_viewing_mixed", false);
            pref.SetBoolPref("security.warn_submit_insecure", false);
            pref.SetIntPref("security.ssl.warn_missing_rfc5746", 1);
            pref.SetBoolPref("security.ssl.enable_false_start", false);
            pref.SetBoolPref("security.enable_ssl3", false);
            pref.SetBoolPref("security.enable_tls", false);
            pref.SetBoolPref("security.enable_tls_session_tickets", false);
            pref.SetIntPref("privacy.popups.disable_from_plugins", 2);

            // don't store passwords
            pref.SetIntPref("security.ask_for_password", 1);
            pref.SetIntPref("security.password_lifetime", 0);
            pref.SetBoolPref("signon.prefillForms", false);
            pref.SetBoolPref("signon.rememberSignons", false);
            pref.SetBoolPref("browser.fixup.hide_user_pass", false);
            pref.SetBoolPref("privacy.item.passwords", true);

            return new GeckoWebBrowser();
            
        }
        private IEnumerable<string> FindFiles(string sDir, string file)
        {
            if (!string.IsNullOrEmpty(sDir) && !string.IsNullOrEmpty(file) && new DirectoryInfo(sDir).Exists)
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d, file))
                    {
                        yield return f;
                    }
                    foreach (string f in this.FindFiles(d, file)) yield return f;
                }
            }
        }
        #endregion
    }
}
