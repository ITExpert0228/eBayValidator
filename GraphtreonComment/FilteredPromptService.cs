using Gecko;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGBot
{
    class FilteredPromptService: nsIPromptService2, nsIPrompt
    {
        public void Alert(string dialogTitle, string text)
        {
            //do nothing, 
            
        }

        public void Alert(nsIDOMWindow aParent, string aDialogTitle, string aText)
        {
            throw new NotImplementedException();
        }

        public void AlertCheck(string dialogTitle, string text, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public void AlertCheck(nsIDOMWindow aParent, string aDialogTitle, string aText, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public nsICancelable AsyncPromptAuth(nsIDOMWindow aParent, nsIChannel aChannel, nsIAuthPromptCallback aCallback, nsISupports aContext, uint level, nsIAuthInformation authInfo, string checkboxLabel, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool Confirm(string dialogTitle, string text)
        {
            throw new NotImplementedException();
        }

        public bool Confirm(nsIDOMWindow aParent, string aDialogTitle, string aText)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmCheck(string dialogTitle, string text, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmCheck(nsIDOMWindow aParent, string aDialogTitle, string aText, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public int ConfirmEx(string dialogTitle, string text, uint buttonFlags, string button0Title, string button1Title, string button2Title, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public int ConfirmEx(nsIDOMWindow aParent, string aDialogTitle, string aText, uint aButtonFlags, string aButton0Title, string aButton1Title, string aButton2Title, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool Prompt(string dialogTitle, string text, ref string value, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool Prompt(nsIDOMWindow aParent, string aDialogTitle, string aText, ref string aValue, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool PromptAuth(nsIDOMWindow aParent, nsIChannel aChannel, uint level, nsIAuthInformation authInfo, string checkboxLabel, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool PromptPassword(string dialogTitle, string text, ref string password, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool PromptPassword(nsIDOMWindow aParent, string aDialogTitle, string aText, ref string aPassword, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool PromptUsernameAndPassword(string dialogTitle, string text, ref string username, ref string password, string checkMsg, ref bool checkValue)
        {
            throw new NotImplementedException();
        }

        public bool PromptUsernameAndPassword(nsIDOMWindow aParent, string aDialogTitle, string aText, ref string aUsername, ref string aPassword, string aCheckMsg, ref bool aCheckState)
        {
            throw new NotImplementedException();
        }

        public bool Select(string dialogTitle, string text, uint count, IntPtr[] selectList, ref int outSelection)
        {
            throw new NotImplementedException();
        }

        public bool Select(nsIDOMWindow aParent, string aDialogTitle, string aText, uint aCount, IntPtr[] aSelectList, ref int aOutSelection)
        {
            throw new NotImplementedException();
        }
        //and so on for other alerts/prompts
    }
}
