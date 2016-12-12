

$.validator.unobtrusive.parse("#form");

$(function () {
    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }
        var ok = true;
        try {
            $.datepicker.parseDate('dd.mm.yy', value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    });
    $(".datepicker").datepicker({ dateFormat: 'dd.mm.yy' });
});

$("#Author_Name").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: "/Author/AuthorSuggestions",
            type: "GET",
            dataType: "json",
            data: { text: request.term },
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.Name, value: item.Name };
                }))
            }
        })
    },
    messages: {
        noResults: "", results: ""
    }
});



$(".form-label").each(function () {
    $(this).removeClass("active");
})

function AuthorCreatedToast() {
    Materialize.toast("<span class=\"toast-info\"><strong>Utworzono nowego autora.</strong>", 3000);
}

function BookCreatedToast() {
    Materialize.toast("<span class=\"toast-info\"><strong>Utworzono nową książkę.</strong>", 3000);
}

function AuthorCreateFailed() {
    Materialize.toast("<span class=\"toast-info\"><strong>Nie można utworzyć autora.</strong>", 3000);
}

function BookCreateFailed() {
        Materialize.toast("<span class=\"toast-info\"><strong>Nie można utworzyć książki.</strong>", 3000);
}



