<%@ Page Language="C#" Title="Venue Booking" MasterPageFile="~/pages/Main.Master" AutoEventWireup="true" CodeBehind="VenueBooking.aspx.cs" Inherits="INSPECTORATEStaff.pages.VenueBooking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="Server">
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Venue Booking
       
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
                            <h3 class="box-title">Venue Booking Lines</h3>
                        </div>
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="vwApply" runat="server">
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-lg-4 col-md-6">
                                            <div class="form-group">
                                                <label>Department</label>
                                                <asp:DropDownList ID="ddlDeparts" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6">
                                            <div class="form-group">
                                                <label>Venue</label>
                                                <asp:DropDownList ID="ddlVenue" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6">
                                            <div class="form-group">
                                                <label>Pax (No. of People)</label>
                                                <asp:TextBox ID="txtPax" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6">
                                            <div class="form-group">
                                                <label>Contact Number</label>
                                                <asp:TextBox ID="txtCntct" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-6">
                                            <div class="form-group">
                                                <label>Email address</label>
                                                <asp:TextBox ID="txtmail" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-4 col-md-6">
                                            <div class="form-group">
                                                <label>Booking Date</label>
                                                <asp:TextBox ID="txtbd" class="form-control" runat="server"></asp:TextBox>
                                                <script>
                                                    $j('#Main1_txtbd').Zebra_DatePicker({
                                                        // remember that the way you write down dates
                                                        // depends on the value of the "format" property!
                                                        direction: [1, false],
                                                        //format: 'Y-m-d h:i A'
                                                    });</script>

                                                <%--<asp:HiddenField runat="server" ID="txtBookingDate" ClientIDMode="Static"></asp:HiddenField>--%>
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-lg-4 col-md-6">
                                            <div class="form-group">
                                                <label>Required Time (hours:minutes)</label>
                                                <asp:TextBox ID="txtrt" class="form-control" runat="server"></asp:TextBox>
                                                <script>
                                                    $j('#Main1_txtrt').Zebra_DatePicker({
                                                        // remember that the way you write down dates
                                                        // depends on the value of the "format" property!
                                                        direction: [1, false],
                                                        format: 'H:i'
                                                    });</script>
                                                <%--<asp:HiddenField runat="server" ID="txtRequiredTime" ClientIDMode="Static"></asp:HiddenField>--%>
                                                <br />
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-md-6">
                                            <div class="form-group">
                                                <label>User ID</label><br />
                                                <asp:Label ID="lbluserId" runat="server" Text="User Id" CssClass="text-info"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-2 col-md-6">
                                            <div class="form-group">
                                                <label>Status</label><br />
                                                <asp:Label ID="lblstatus" runat="server" Text="Status" CssClass="text-success"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12">
                                            <label>Description of Meeting</label>
                                            <asp:TextBox ID="txtDesc" class="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                                <%--<label for="atext3">Days applied</label>--%>
                                                <div class="col-lg-12 col-md-12">
                                                    <asp:LinkButton ID="lbtnApply" class="btn btn-line btn-info" runat="server" OnClick="lbtnApply_Click"> <i class="fa fa-car"></i> Submit</asp:LinkButton>
                                                    <%--<asp:LinkButton ID="lbtnBack" class="btn btn-line btn-danger" runat="server"> <i class="fa fa-backward"></i> Cancel</asp:LinkButton>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:View>
                            <asp:View ID="vwDone" runat="server">
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <table class="table table-hover">
                                                <thead>
                                                    <tr>
                                                        <th class="small">Ref No</th>
                                                        <th class="small">Description</th>
                                                        <th class="small">Venue</th>
                                                        <th class="small">Request Date</th>
                                                        <th class="small">Booking Date</th>
                                                        <th class="small">Required Time</th>
                                                        <th class="small">No of People</th>
                                                        <th class="small">Status</th>
                                                        <%--<th class="small">Action</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <%=Jobs()%>
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
                        <p>Your Venue booking is successfully created.</p>
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

