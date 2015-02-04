<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ConfigureApplicationInsights.ascx.cs" Inherits="Engage.Dnn.ApplicationInsights.ConfigureApplicationInsights" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<asp:Panel runat="server" CssClass="dnnForm" DefaultButton="SubmitButton">
    <div class="dnnFormItem">
        <dnn:Label runat="server" ResourceKey="InstrumentationKey" />
        <asp:TextBox runat="server" Text="<%#Model.InstrumentationKey %>" ID="InstrumentationKeyTextBox" />
    </div>
    <ul class="dnnClear dnnActions">
        <li>
            <asp:Button ID="SubmitButton" runat="server" CssClass="dnnPrimaryAction" ResourceKey="Submit" OnClick="SubmitButton_Click" />
        </li>
    </ul>
</asp:Panel>