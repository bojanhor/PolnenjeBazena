function MoveNext(next_ctrl, _key) {
    if (_key === 13) // if enter key
    {
        var _next = document.getElementById(next_ctrl);
        _next.focus();
        return false;
    }
}
