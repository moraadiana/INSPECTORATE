<%@ Page Language="C#" Title="Leave application" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="LeaveApplication.aspx.cs" Inherits="INSPECTORATEStaff.pages.LeaveApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Leave Application
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Leave Application</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <!-- SELECT2 EXAMPLE -->
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i> New Leave Application</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Leave Type</label>
                                        <asp:DropDownList ID="DdLeaveType" class="form-control select2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DdLeaveType_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:ListItem Value="1" Text="-----Select Leave Type-----"></asp:ListItem>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Reliever1</label>
                                       <asp:DropDownList ID="ddlReliever" class="form-control select2" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Reliever2</label>
                                       <asp:DropDownList ID="ddlReliever2" class="form-control select2" runat="server">
                                           <asp:ListItem Value="1" Text="-----Select Reliever-----"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Reliever3</label>
                                       <asp:DropDownList ID="ddlReliever3" class="form-control select2" runat="server">
                                           <asp:ListItem Value="1" Text="-----Select Reliever-----"></asp:ListItem>
                                       </asp:DropDownList>
                                    
                                    </div>
                                </div>
                                  <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Responsibility Center</label>
                                        <asp:DropDownList ID="ddlresponibilitycentres" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>Leave Balance:</label>
                                         <asp:Label ID="lblLeaveBal" ForeColor="Blue" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                             <div class="row">                                
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Days applied</label>
                                        <asp:TextBox ID="TxtAppliedDays" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Start Date</label>
                                        <asp:TextBox ID="txtStartDate" class="form-control" runat="server" onkeyup="RefreshUpdatePanel();" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                            <script>
                                                $j('#Main1_txtStartDate').Zebra_DatePicker({
                                                    direction: [1, false],
                                                    onSelect: function () {
                                                        this.trigger("change")
                                                    }
                                                });
                                                //$(function () {  //On document.ready
                                                //    var textbox = $("input[id$='txtStartDate']"); //ends with selector (or you can use ASP to get the id)
                                                //    $(textbox).change(function (e) {
                                                //        __doPostBack(this.attr("id"), e); //Call postback manually
                                                //    });
                                                //});
                                            </script>
                                    </div>
                                </div>
                               <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:Panel runat="server" ID="panel1" Width="203px">
                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="End Date" CssClass=""></asp:Label>
                                            <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" ForeColor="#FF6600" CssClass="spLabel">&nbsp;</asp:Label>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <asp:Panel runat="server" ID="panel2" Width="225px">
                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Return Date"></asp:Label>
                                            <asp:Label ID="lblReturnDate" runat="server" Font-Bold="True" ForeColor="#FF6600" CssClass="spLabel">&nbsp;</asp:Label>
                                        </asp:Panel>
                                    </div>
                                </div>
                                 <div class="col-sm-12">
                                <div class="form-group">
                                    <label for="atext3">Purpose</label>
                                    <div class="icon-after-input">
                                        <asp:TextBox ID="TxtPurpose" class="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            </div>
                                 <div class="row">
                                     <div class="col-md-4">
                                         <label>Attach Signed Leave Document</label>
                                         <asp:FileUpload ID="fuLeaveDocs" CssClass="form-control" ToolTip="Attach documents with .pdf, .png, .jpg and .jpeg extensions only" runat="server" />
                                         <br />
                                         <asp:LinkButton ID="lbtnAttach" runat="server" CssClass="btn btn-primary" OnClick="lbtnAttach_Click"><i class="fa fa-upload"></i>&nbsp;Upload</asp:LinkButton>
                                     </div>
                                 </div>
                            <br />
                            <h4>Document Attachments</h4>
<asp:GridView ID="gvAttachments" AutoGenerateColumns="false" DataKeyNames="Document No" class="table table-responsive no-padding table-bordered table-hover" runat="server"
    AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="5">
    <Columns>
        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
            <HeaderStyle Width="30px" />
            <ItemTemplate>
                <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Document No" HeaderText="Document No" />
        <asp:BoundField DataField="Description" HeaderText="Description" />
        <asp:BoundField DataField="$systemCreatedAt" HeaderText="Date Uploaded" />
        <asp:TemplateField HeaderText="Action" SortExpression="" HeaderStyle-HorizontalAlign="Left">
            <ItemStyle Width="110px" HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:LinkButton ID="lbtnRemoveAttach" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="lbtnRemoveAttach_Click" OnClientClick="return confirm('Are you sure you want to delete this line?')" CommandArgument='<%# Eval("SystemId") %>'><i class="fa fa-remove"></i> Remove</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle HorizontalAlign="Center" />
    <EmptyDataTemplate>
        <span style="color: red">No Recods</span>
    </EmptyDataTemplate>
</asp:GridView>
                             
                            
                            
                            
                            <!-- /.box-body -->
                               <div class="box-footer clearfix">
                                     <asp:LinkButton ID="lbtnApply" class="btn btn-sm btn-success btn-flat pull-right" runat="server" OnClick="lbtnApply_Click"> <i class="fa fa-send-o"></i> &nbsp;Submit</asp:LinkButton>
                                        <asp:LinkButton ID="lbtnBack" class="btn btn-sm btn-warning btn-flat pull-left" runat="server" OnClick="lbtnBack_Click"> <i class="fa fa-backward"></i> Back</asp:LinkButton>
                        
                        </div>
                        </div>
                        <!-- /.box -->
                    </div>
                </div>
            </div>
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
       <!-- BEGIN: modal -->
    <div class="modal fade" id="eventModal" tabindex="-1" role="dialog" aria-labelledby="eventModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content bg-emerland"  runat="server" id="dvMdlContentPass"  visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabel">Success!!</h4>
                </div>
                <div class="modal-body">
                    <div>
                         <p>Your leave Request has been successfully sent for Approval.</p>
                    </div>
                </div>
            </div>
              <div class="modal-content bg-alizarin text-white"  runat="server" id="dvMdlContentFail" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabelf">Error!</h4>
                </div>
                <div class="modal-body">
                    <div>
                         <p id="pMsg" runat="server">Oparation failed.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END: modal -->
</asp:Content>

