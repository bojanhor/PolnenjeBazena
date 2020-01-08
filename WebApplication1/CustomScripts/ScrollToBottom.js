function ScrollToBottom() {
    //this function is forcing scrollbar of textbox to go to bottom position after postback
    var yPos;
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler);
    function BeginRequestHandler(sender, args) {
        
    }
    function EndRequestHandler(sender, args) {
        yPos = $get('TextBoxDebugID').scrollHeight + 10;
        $get('TextBoxDebugID').scrollTop = yPos +1;
    }

}

ScrollToBottom();



