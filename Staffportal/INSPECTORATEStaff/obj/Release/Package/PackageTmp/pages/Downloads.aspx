<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Downloads.aspx.cs" Inherits="INSPECTORATEStaff.pages.Downloads" Title="Downloads" MasterPageFile="~/pages/Main.Master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="page-content" style="background-color:aliceblue;padding:10px;">
        <div class="page-header">
            <h1>INSPECTORATE DOWNLOAD Documents </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-12 widget-container-col">
                <div class="widget-box widget-color-orange" id="widget-box-3">
                    <div class="widget-header widget-header-small">
                        <h6 class="widget-title">
                            <i class="ace-icon fa fa-sort"></i>
                            INSPECTORATE DOWNLOAD Documents
                        </h6>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main">
                           <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" Width="100%" CellPadding="4"
                                    CssClass="table table-hover" GridLines="None">
                                    <Columns>
                                          <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#" SortExpression="">
                                            <HeaderStyle Width="30px" />
                                            <ItemTemplate>
                                                <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Text" HeaderText="File Name" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("Value") %>' class="label label-warning" runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
    </div>
    <!-- /.page-content -->
</asp:Content>