function RetainScrollPosition() {
    //this function is forcing scrollbar of textbox to retain position after postback
    var yPos;
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler);
    function BeginRequestHandler(sender, args) {
        yPos = $get('TextBoxDebugID').scrollTop;
    }
    function EndRequestHandler(sender, args) {
        $get('TextBoxDebugID').scrollTop = yPos;
    }

}

RetainScrollPosition();



