var template;
var groceries = {};

groceries.loadList = function () {
    $.getJSON('api/Grocery/', function (data) {
        var result = { results: data };

        $('#grocerylist').html(template(result));
        groceries.rebindList();
    });
};

groceries.addItem = function (item, section, done) {
    var data = { Item: item, Section: section, Done: done };
    $.post('api/Grocery/', data, function () {
        $('#newitem').val('');
        groceries.loadList();
    });
};

groceries.updateItem = function (id, item, section, done) {
    var data = { Id: id, Item: item, Section: section, Done: done };
    $.ajax({
        type: 'PUT',
        url: 'api/Grocery/' + id,
        data: data
    }).done(function () {
        groceries.loadList();
    });
};

groceries.clearDeleted = function () {
    $.ajax({
        type: 'DELETE',
        url: 'api/Grocery/'
    }).done(function () {
        groceries.loadList();
    });
};

groceries.rebindList = function () {
    $('.itemview').on("dblclick", function (event) {
        $(this).parent().children('.itemview').addClass('hidden');
        $(this).parent().children('.itemedit').removeClass('hidden');
    });

    $('.itemupdate').on("click", function () {
        var item = $(this).parent().children('.changeitem').val();
        var section = $(this).parent().children('.changesection').val();
        var id = $(this).parent().children('.itemid').val();

        groceries.updateItem(id, item, section, false);
    });

    $('.itemchecked').on("click", function (event) {
        var id = $(this).parent().parent().children('.itemedit').children('.itemid').val();
        var item = $(this).parent().parent().children('.itemedit').children('.changeitem').val();
        var section = $(this).parent().parent().children('.itemedit').children('.changesection').val();

        groceries.updateItem(id, item, section, !$(this).parent().hasClass('done'));
    });

    $('.changeitem,.changesection').keypress(function (e) {
        if (e.which == 13) {
            $(this).parent().children('.itemupdate').click();
        }
    });  
};

$(document).ready(function () {
    template = Handlebars.compile($('#grocery-template').html());

    $('#addnew').click(function () {
        if ($('#newitem').val() != '') {
            groceries.addItem($('#newitem').val(), "", false);
        }
    });

    $('#newitem').keypress(function (e) {
        if (e.which == 13) {
            $('#addnew').click();
        }
    });

    $('#cleardeleted').click(function () {
        groceries.clearDeleted();
    });
    
    groceries.loadList();
});