<%@ Control Language="VB" AutoEventWireup="true" Inherits="MainClaimData" Strict="false" Explicit="false"   Codebehind="MainClaimData.ascx.vb" %>
<%@ Register assembly="DevExpress.Web.v21.2" namespace="DevExpress.Web" tagprefix="dx" %>




<head>

    <link href="../Styles/MainSiteStyle.css" rel="stylesheet" type="text/css" />
 <script  type="text/javascript" src="../../Scripts/jquery-latest.min.js"></script>

</head>
	<script lang="javascript" type="text/javascript">
	    $(document).ready(function () {
	        //debugger;
	        // Setting focus on first textbox

	        $('input:text:first').focus();

	        // binding keydown event to textbox

	        $('input:text').bind('keydown', function (e) {

	            // detecting keycode returned from keydown and comparing if its equal to 13 (enter key code)

	            if (e.keyCode == 13) {

	                // by default if you hit enter key while on textbox so below code will prevent that default behaviour

	                e.preventDefault();

	                // getting next index by getting current index and incrementing it by 1 to go to next textbox

	                var nextIndex = $('input:text').index(this) + 1;

	                // getting total number of textboxes on the page to detect how far we need to go

	                var maxIndex = $('input:text').length;

	                // check to see if next index is still smaller then max index

	                if (nextIndex < maxIndex) {

	                    // setting index to next textbox using CSS3 selector of nth child

	                    $('input:text:eq(' + nextIndex + ')').focus();

	                }



	            }

	        });

	    });

// <!CDATA[


// ]]>
</script>


<div dir="rtl" >
            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" RightToLeft="True" runat="server" Width="100%" HeaderText="بيانات الحادث الرئيسية" CssFilePath="~/App_Themes/PlasticBlue/{0}/styles.css" CssPostfix="PlasticBlue" GroupBoxCaptionOffsetY="-18px" SpriteCssFilePath="~/App_Themes/PlasticBlue/{0}/sprite.css" >
                <contentpaddings paddingbottom="8px" />
                <PanelCollection>
                    <dx:PanelContent>
                        <table style="width:100%;">
                            <tr>
                                <td class="auto-style5" style="text-align: left">رقم الحادث</td>
                                <td class="auto-style6">
                                    <dx:ASPxTextBox ID="ClmNo" runat="server" ClientInstanceName="ClmNo" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style5" style="text-align: left">تاريخ الحادث</td>
                                <td class="auto-style6">
                                    <dx:ASPxTextBox ID="ClmDate" runat="server" ClientInstanceName="ClmDate" Width="100px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style5" style="text-align: left">تاريخ البلاغ</td>
                                <td class="auto-style6">
                                    <dx:ASPxTextBox ID="ClmInfDate" runat="server" ClientInstanceName="ClmInfDate" Width="100px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style5">&nbsp;</td>
                                <td class="auto-style6">
                                    &nbsp;</td>
                                <td class="auto-style5">&nbsp;</td>
                                <td class="auto-style6">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxtcLeftAlignCell_PlasticBlue">اسم المؤمن له</td>
                                <td colspan="5">
                                    <dx:ASPxTextBox ID="CustName" runat="server" ClientInstanceName="CustName" Width="100%">
                                    </dx:ASPxTextBox>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style5" style="text-align: left">رقم الوثيقة</td>
                                <td class="auto-style6">
                                    <dx:ASPxTextBox ID="PolNo" runat="server" ClientInstanceName="PolNo" Width="170px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style5" style="text-align: left">رقم الملحق</td>
                                <td class="auto-style6">
                                    <dx:ASPxTextBox ID="EndNo" runat="server" ClientInstanceName="EndNo" Width="30px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style5" style="text-align: left">رقم الإشعار</td>
                                <td class="auto-style6">
                                    <dx:ASPxTextBox ID="LoadNo" runat="server" ClientInstanceName="LoadNo" Width="30px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style5">&nbsp;</td>
                                <td class="auto-style6">
                                    &nbsp;</td>
                                <td class="auto-style5">&nbsp;</td>
                                <td class="auto-style6">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="auto-style7" colspan="0">&nbsp;</td>
                                <td class="auto-style7" style="text-align: left">التغطية</td>
                                <td class="auto-style7" style="text-align: left">من </td>
                                <td class="auto-style7">
                                    <dx:ASPxTextBox ID="CoverFrom" runat="server" ClientInstanceName="CoverFrom" Width="100px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style7" style="text-align: left">إلى</td>
                                <td class="auto-style7">
                                    <dx:ASPxTextBox ID="CoverTo" runat="server" ClientInstanceName="CoverTo" Width="100px">
                                    </dx:ASPxTextBox>
                                </td>
                                <td class="auto-style7" colspan="0">&nbsp;</td>
                                <td class="auto-style1" colspan="0">
                                    &nbsp;</td>
                                <td class="auto-style7" colspan="0">&nbsp;</td>
                                <td class="auto-style1" colspan="0">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="dxtcLeftAlignCell_PlasticBlue">ملخص الأضرار</td>
                                <td colspan="9">
                                    <dx:ASPxMemo ID="DmgDiscription" runat="server" AutoResizeWithContainer="True" Height="93px" RightToLeft="True" Width="100%">
                                    </dx:ASPxMemo>
                                </td>
                            </tr>
                            <tr>
                                <td class="dxtcLeftAlignCell_PlasticBlue">التسويات&nbsp; التي تخص </td>
                                <td colspan="9">
                                    <asp:Label ID="Label1" runat="server" ></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
</div>
            
            

            

            




