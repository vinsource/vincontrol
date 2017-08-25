// JScript File

function checkDate(sender,args)
{
 if (sender._selectedDate < new Date())
    {
    alert("You cannot select a day earlier than today!");
    sender._selectedDate = new Date(); 
    // set the date back to the current date
    sender._textbox.set_Value(sender._selectedDate.format(sender._format))
    }
}




    