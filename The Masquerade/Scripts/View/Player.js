function disableEmail(value) {
    if (value == "Player")
    {
        $("#Player_eMail").prop("disabled", false);
    }
    else
    {
        $("#Player_eMail").prop("disabled", true);
    }
}