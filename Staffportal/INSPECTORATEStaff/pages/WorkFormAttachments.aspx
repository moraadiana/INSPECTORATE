<%@ Page Title="WorkForm Attachments" Language="C#" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="WorkFormAttachments.aspx.cs" Inherits="INSPECTORATEStaff.pages.WorkFormAttachments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="page-content" style="background-color: aliceblue; padding: 20px;">
        <div class="page-header mb-4">
            <h1 class="text-primary">Work Forms Attachments</h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-12 widget-container-col">
                <div class="widget-box widget-color-orange shadow-sm" id="widget-box-3" style="border-radius: 5px;">
                    <div class="widget-header widget-header-small bg-orange text-white d-flex align-items-center" style="border-top-left-radius: 5px; border-top-right-radius: 5px;">
                        <h6 class="widget-title m-0">
                            <i class="ace-icon fa fa-sort mr-2"></i>
                            Work Forms Attachments
                        </h6>
                    </div>

                    <div class="widget-body bg-white" style="border-bottom-left-radius: 5px; border-bottom-right-radius: 5px;">
                        <div class="widget-main p-3">
                            <div class="row mb-3">
                                <div class="col-md-12">
                                    <h3 class="h5 font-weight-bold">Document Attachments</h3>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvAttachments" AutoGenerateColumns="false" DataKeyNames="Document No"
                                        CssClass="table table-bordered table-hover table-responsive"
                                        runat="server" AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5">
                                        <Columns>
                                            <asp:TemplateField HeaderText="#No" HeaderStyle-HorizontalAlign="Center" SortExpression="">
                                                <HeaderStyle Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# (Container.DataItemIndex + 1) + "." %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Document No" HeaderText="Document No" />
                                            <asp:BoundField DataField="File Name" HeaderText="File Name" />
                                            <%-- <asp:BoundField DataField="$systemCreatedAt" HeaderText="Date Uploaded" /> --%>
                                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                                <ItemStyle Width="120px" HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                   <%-- <asp:LinkButton ID="lbtnDownload" CssClass="btn btn-sm btn-danger"
                                                        runat="server" ToolTip="Click to Download" OnClick="lbtnDownload_Click">
                                                        <i class="fa fa-download"></i> Download
                                                    </asp:LinkButton>--%>
                                                    <asp:LinkButton ID="lbtnDownload" Text="Download" CommandArgument='<%# Eval("File Name") %>' class="label label-warning" runat="server" OnClick="lbtnDownload_Click"></asp:LinkButton>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <EmptyDataTemplate>
                                            <div class="text-center text-danger py-3">
                                                <em>No Records found.</em>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row mt-3">
                                <div class="col-md-12">
                                    <a href="WorkFormsListing.aspx" class="btn btn-warning">
                                        <i class="fa fa-arrow-left"></i>&nbsp;Back
                                    </a>
                                    <%-- Uncomment if submit is needed
                                    <asp:LinkButton ID="lbtnSubmit" runat="server" CssClass="btn btn-success float-right" OnClick="lbtnSubmit_Click">
                                        <i class="fa fa-paper-plane"></i>&nbsp;Submit
                                    </asp:LinkButton>
                                    --%>
                                </div>
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
