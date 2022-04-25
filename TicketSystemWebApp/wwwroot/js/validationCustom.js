function validationCustom(fields, submit) {
    fields.forEach(element => {
        // Get HTML element.
        var field = document.querySelector(element);

        // Get the last child for parent (span field with error message).
        var errorField = field.parentElement.lastElementChild;

        // Hide error message.
        errorField.style.display = 'none';

        // Add event for HTML elements.
        field.addEventListener('input', (e) => { validationField(field) });
    });

    // Get button and add event to it.
    var button = document.querySelector(submit);
    button.addEventListener("click", (e) => { validationSubmit(e, fields) });
};


function validationField(field) {
    // Get the last child for parent (span field with error message).
    var errorField = field.parentElement.lastElementChild;

    // Verification if field is empty.
    if ($.trim(field.value) == '') {
        errorField.style.display = 'block';
    }
    else {
        errorField.style.display = 'none';
    }
};

function validationSubmit(e, fields) {
    // Array for statuses;
    var statuses = [];

    fields.forEach(element => {
        // Get HTML element.
        var field = document.querySelector(element);

        // Get the last child for parent ('span' HTML field with error message).
        var errorField = field.parentElement.lastElementChild;

        // Verification if field is empty.
        if ($.trim(field.value) == '') {
            errorField.style.display = 'block';
            statuses.push(false)
        }
        else {
            errorField.style.display = 'none';
            statuses.push(true)
        }
    });

    // Verficaton if the array includes any entry with 'false'
    if (statuses.includes(false)) {
        //  Don't action after click the button.
        e.preventDefault();
    }
};