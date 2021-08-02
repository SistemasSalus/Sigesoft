$(function () {

    //var viewModel = new ViewModel();
    //ko.applyBindings(viewModel);

    var viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    function ViewModel() {

        //Make the self as 'this' reference
        var self = this;

        self.HeadquarterPerCompanies = ko.observableArray();

        self.addHeadquarter = function () {
            self.HeadquarterPerCompanies.push(new Item());
        };

        var companyId = document.getElementById('CompanyId').value;

        var input = {
            id: companyId
        };

        $.ajax({
            url: '/Company/HeadquarterPerCompanyByCompanyId',
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: input,
            dataType: "json",
            success: function (data) {

                console.log(data);

                if (self.HeadquarterPerCompanies().length > 0)
                    self.HeadquarterPerCompanies.removeAll();

                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        self.HeadquarterPerCompanies.push(new SetHeadquarter(data[i]));
                    }
                }

            }
        }).fail(
            function (xhr, textStatus, err) {
                alert(err);
            });

        self.addHeadquarter = function () {
            self.HeadquarterPerCompanies.push(new Item());
        };

        self.deleteHeadquarter = function (item) {
            //var headquarterId = ko.toJSON(item.HeadquarterId.);
            //console.log(item);

            //console.log(input);
            console.log(item);

            //$.ajax({
            //    url: '/Company/DeleteHeadquarterPerCompany/' + item.HeadquarterId,
            //    type: 'POST',
            //    //contentType: 'application/json; charset=utf-8',
            //    //data: input,
            //    //dataType: "json",
            //    success: function (data) {


            //        self.HeadquarterPerCompanies.remove(item);
            //    }
            //}).fail(
            //    function (xhr, textStatus, err) {
            //        alert(err);
            //    });

            //self.HeadquarterPerCompanies.remove(item);
            //if (self.HeadquarterPerCompanies().length === 0)
            //    self.addHeadquarter();
        };

        //console.log(self.HeadquarterPerCompanies().length)



        self.errors = ko.validation.group(this, { deep: true, observable: false });

        self.submit = function () {
            if (self.errors().length === 0)
                return true;

            self.errors.showAllMessages();
            return false;
        };




    }

    function getGuid(str) {
        return str.slice(0, 8) + "-" + str.slice(8, 12) + "-" + str.slice(12, 16) +
            "-" + str.slice(16, 20) + "-" + str.slice(20, str.length + 1)
    }


    function SetHeadquarter(data) {

        var self = this;

        self.HeadquarterId = ko.observable(data.HeadquarterId).extend({ required: false });
        self.CompanyId = ko.observable(data.CompanyId).extend({ required: false });
        self.Department = ko.observable(data.Department).extend({ required: { message: '' } });
        self.Province = ko.observable(data.Province).extend({ required: { message: '' } });
        self.District = ko.observable(data.District).extend({ required: { message: '' } });
        self.HeadquarterAddress = ko.observable(data.HeadquarterAddress).extend({ required: false });
        self.HeadquarterPhone = ko.observable(data.HeadquarterPhone).extend({ required: false });

    }

    function Item() {
        var self = this;

        var companyId = document.getElementById('CompanyId').value;

        self.HeadquarterId = ko.observable("").extend({ required: false });
        self.CompanyId = ko.observable(companyId).extend({ required: false });
        self.Department = ko.observable("").extend({ required: { message: '' } });
        self.Province = ko.observable("").extend({ required: { message: '' } });
        self.District = ko.observable("").extend({ required: { message: '' } });
        self.HeadquarterAddress = ko.observable("").extend({ required: { message: '' } });
        self.HeadquarterPhone = ko.observable("").extend({ required: { message: '' } });
    }
});