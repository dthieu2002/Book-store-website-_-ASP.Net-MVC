// My JS
function checkRegister() {
    let username = document.getElementById('Username').value;
    let password = document.getElementById('Password').value;
    let passwordConfirm = document.getElementById('PasswordConfirm').value;

    // password and password confirm
    if (password != passwordConfirm) {
        swal("ERROR!", "Password and password confirm is not same!", "error");
        return;
    }

    //Redirect to check acount by POST method ( check backend)
    var form = $('<form aciton="DTHBookStore/Register" method="post">' +
            '<input type="text" name="Username" value="' + username + '" />' +
            '<input type="text" name="Password" value="' + password + '" />' +
        '</form>');
    $('body').append(form);
    form.submit();
}

