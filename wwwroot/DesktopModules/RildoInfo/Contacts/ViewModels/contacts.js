$(function () {

    my.viewModel();
    
    my.vm.loadCountries();
    my.vm.loadRegions();

    $('#btnConfig').click(function (e) {
        e.preventDefault();

        document.location.href = _optionsURL;
    });

});