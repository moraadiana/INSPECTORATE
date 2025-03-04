<%@ Page Title="" Language="C#" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="TrainingApplication.aspx.cs" Inherits="INSPECTORATEStaff.pages.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Training Application
       
                <%--<small>Version 2.0</small>--%>
            </h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Venue Booking</li>
            </ol>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-warning box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">Transport Requisition Lines</h3>
                        </div>
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="vwApply" runat="server">
                                <div class="box-body collapse in">
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label for="atext">Training Code</label>
                                                <div class="icon-after-input">
                                                    <asp:DropDownList ID="ddlCode" class="form-control select2" runat="server">
                                                        <asp:ListItem Value="1">UF34635</asp:ListItem>
                                                        <asp:ListItem Value="2">UF9857</asp:ListItem>
                                                        <%--<asp:ListItem Value="3">Club/Societies</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label for="atext2">Department:</label>
                                                <div class="icon-after-input">
                                                    <asp:DropDownList ID="ddlGroup" class="form-control select2" runat="server">
                                                        <asp:ListItem Value="1">ICT</asp:ListItem>
                                                        <asp:ListItem Value="2">Finance</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label for="atext2">Number of Days:</label>
                                                <div class="icon-after-input">
                                                    <asp:TextBox ID="txtDays" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label for="atext2">Number of Passangers:</label>
                                                <div class="icon-after-input">
                                                    <asp:TextBox ID="txtPassa" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label for="atext3">From: </label>
                                                <div class="icon-after-input">
                                                    <asp:TextBox ID="txtFrom" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <div class="form-group">
                                                <label for="atext3">Destination</label>
                                                <div class="icon-after-input">
                                                    <asp:TextBox ID="txtDestination" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label for="atext3">Date of Trip: </label>
                                                <div class="icon-after-input">
                                                    <asp:TextBox ID="txtDateofTrip" class="form-control" runat="server"></asp:TextBox>
                                                    <script>
                                                        $j('#Main1_txtDateofTrip').Zebra_DatePicker({
                                                            // remember that the way you write down dates
                                                            // depends on the value of the "format" property!
                                                            direction: [1, false],
                                                        });</script>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <label for="atext3">Purpose of trip</label>
                                            <div class="icon-after-input">
                                                <asp:TextBox ID="TxtPurpose" class="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <%--<label for="atext3">Days applied</label>--%>
                                            <div class="icon-after-input">
                                                <asp:LinkButton ID="lbtnApply" class="btn btn-line btn-info" runat="server" OnClick="lbtnApply_Click"> <i class="fa fa-car"></i> Submit</asp:LinkButton>
                                                <%--<asp:LinkButton ID="lbtnBack" class="btn btn-line btn-danger" runat="server"> <i class="fa fa-backward"></i> Cancel</asp:LinkButton>--%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:View>
                            <asp:View ID="vwDone" runat="server">
                                <div class="box-body collapse in">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <table class="table table-hover">
                                                <thead>
                                                    <tr>
                                                        <th class="small">Ref No</th>
                                                        <th class="small">Commencement</th>
                                                        <th class="small">Destination</th>
                                                        <th class="small">Vehicle Allocated</th>
                                                        <th class="small">Driver Allocated</th>
                                                        <th class="small">Date of Request</th>
                                                        <th class="small">Date of Trip</th>
                                                        <th class="small">No of Days Requested</th>
                                                        <%--<th class="small">No of Passengers</th>--%>
                                                        <th class="small">Status</th>
                                                        <%--<th class="small">Action</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%--<%=Jobs()%>--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="col-sm-12">
                                        <div class="form-group">
                                            <%--<label for="atext3">Days applied</label>--%>
                                            <div class="icon-after-input">
                                                <asp:LinkButton ID="lbtnApproval" class="btn btn-line btn-info" runat="server" OnClick="lbtnApproval_Click"> <i class="fa fa-car"></i> Send for Approval</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnCancel" class="btn btn-line btn-warning" runat="server" OnClick="lbtnCancel_Click"> <i class="fa fa-car"></i> Cancel Approval Request</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnCAll" class="btn btn-line btn-danger pull-right" runat="server" OnClick="lbtnCAll_Click"> <i class="fa fa-car"></i> Cancel Transport Request</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
    </div>
        </section>
    </div>
      <!-- BEGIN: modal -->
    <div class="modal fade" id="eventModal" tabindex="-1" role="dialog" aria-labelledby="eventModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content bg-emerland" runat="server" id="dvMdlContentPass" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabel">Success!!</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <p>Your Travel Request is successfully created.</p>
                    </div>
                </div>
            </div>
            <div class="modal-content bg-alizarin text-white" runat="server" id="dvMdlContentFail" visible="false">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="eventModalLabelf">Error!!</h4>
                </div>
                <div class="modal-body">
                    <div>
                        <p id="pMsg" runat="server">The operation has failed.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END: modal -->
</asp:Content>

