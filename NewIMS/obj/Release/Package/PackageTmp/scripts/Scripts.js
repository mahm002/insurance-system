function NewPolicy(Pag, Sy) {
    PrintPop.SetSize(700, 800);
    PrintPop.SetHeaderText('وثيقة جديدة');
    PrintPop.SetContentHtml(null);
    PrintPop.SetContentUrl(Pag + '?Sys=' + Sy);
    PrintPop.Show();
}

//Registration
function OnBackButtonClick() {
    pc.SetActiveTabIndex(pc.GetActiveTabIndex() - 1);
    dxpError.SetVisible(false);
    UpdateButtonsEnabled();
}
function OnNextButtonClick() {
    var tabName = pc.GetActiveTab().name;
    var areEditorsValid = ASPxClientEdit.ValidateEditorsInContainerById(tabName);
    if (areEditorsValid) {
        var nextTab = pc.GetTab(pc.GetActiveTabIndex() + 1);
        nextTab.SetEnabled(true);
        pc.SetActiveTab(nextTab);
    }
    dxpError.SetVisible(!areEditorsValid);
    UpdateButtonsEnabled();
}
function OnFinishButtonClick(s, e) {
    var finishTab = pc.GetTabByName('Finish');
    DisableRegistrationTabs();
    finishTab.SetEnabled(true);
    pc.SetActiveTab(finishTab);
    UpdateButtonsEnabled();
}
function OnTick(s, e) {
    //previous = startProgress;
    //startProgress = function () { };
    //cbp.PerformCallback();
    //startProgress = previous;
}

function SaveRegField(sender, fieldName) {
    SetRegField(fieldName, sender.GetValue());
}
function LoadRegField(sender, fieldName) {
    return sender.SetText(GetRegField(fieldName));
}

function GetRegField(fieldName) {
    return jQuery.parseJSON(GetRegData())[fieldName];
}
function SetRegField(fieldName, value) {
    var dataString = GetRegData();
    var data = jQuery.parseJSON(dataString);
    data[fieldName] = value;
    SaveRegData(data);
}

function GetRegData() {
    return hfRegInfo.Get('RegData').toString();
}
function SaveRegData(value) {
    hfRegInfo.Set('RegData', ASPx.Json.ToJson(value));
}

//Validation

function IsExpirationDateFilled() {
    return IsDateEditorValueSetted(cmbCardExpirationMonth) && IsDateEditorValueSetted(cmbCardExpirationYear);
}

function ValidateCardExpirationDate() {
    var date = new Date();
    return cmbCardExpirationYear.GetValue() > date.getFullYear() ||
        cmbCardExpirationMonth.GetValue() >= date.getMonth() + 1;
}

//schedule
function ChangeSheduleDay(s, e) {
    ASPxScheduler1.GotoDate(new Date(Date.parse(s.tabs[e.tab.index].name)));
}
function SheduleDayTabClick(s, e) {
    e.cancel = ASPxScheduler1.InCallback();
}
function ShowSelectedAppointmentDetails() {
    ASPxScheduler1.ShowAppointmentFormByServerId(ASPxScheduler1.GetSelectedAppointmentIds()[0]);
}
function ChangeAppointmentLabel(aptId, value) {
    apt = ASPxScheduler1.GetAppointmentById(aptId);
    apt.SetLabelId(value);
    ASPxScheduler1.UpdateAppointment(apt);
}
function ResizeSwitcher(s, e) {
    var sizes = [115, 143, 93];
    s.SetWidth(sizes[s.GetValue()]);
}
function OnReportToolbarItemValueChanged(s, e) {
    cbChangePrintingStatus.PerformCallback(s.GetValue());
}
function OnEndChangePrintingStatusCallback(s, e) {
    ClientReportViewer.Refresh();
}

function OnPageControlActiveTabChanged(s, e) {
    UpdatePrevNextEnabledState();
}

// Prev/Next Buttons
function OnNextButtonClick(s, e) {
    SetActiveTab(1);
    UpdatePrevNextEnabledState();
}
function OnPrevButtonClick(s, e) {
    SetActiveTab(-1);
    UpdatePrevNextEnabledState();
}

function SetActiveTab(tabIndexIncrement) {
    var activeTabIndex = PageControl.GetActiveTab().index;
    activeTabIndex += tabIndexIncrement;
    PageControl.SetActiveTab(PageControl.GetTab(activeTabIndex));
}
function UpdatePrevNextEnabledState() {
    var activeTabIndex = PageControl.GetActiveTab().index;
    PrevButton.SetEnabled(activeTabIndex > 0);
    NextButton.SetEnabled(activeTabIndex < PageControl.GetTabCount() - 1);
}

// Submit Button
function OnSumbitButtonClick(s, e) {
    var tabPageCount = PageControl.GetTabCount();
    for (var i = 0; i < tabPageCount; i++) {
        PageControl.SetActiveTab(PageControl.GetTab(i));
        var editorsContainerId = "tblContainer" + i;
        if (!ASPxClientEdit.ValidateEditorsInContainerById(editorsContainerId)) {
            e.processOnServer = false;
            break;
        }
    }
}