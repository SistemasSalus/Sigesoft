$(function () {

    //var viewModel = new ViewModel();
    //ko.applyBindings(viewModel);

    var viewModel = new ViewModel();
    ko.applyBindings(viewModel);

    function ViewModel() {

        //Make the self as 'this' reference
        var self = this;
        

        self.IdentificationNumber = ko.observable("").extend({ required: { message: ' ' } });
        self.CompanyName = ko.observable("").extend({ required: { message: ' ' } });
        self.CompanyAddress = ko.observable("").extend({ required: { message: ' ' } });        
        self.CompanyPhone = ko.observable("").extend({ required: false });
        self.ContactName = ko.observable("").extend({ required: false });
        self.Department = ko.observable("").extend({ required: false });
        self.District = ko.observable("").extend({ required: false });
        self.Province = ko.observable("").extend({ required: false });        
        self.ContactName = ko.observable("").extend({ required: { message: ' ' } });
        self.ContactPhoneNumber = ko.observable("").extend({ required: false });
      
        self.HeadquarterPerCompanies = ko.observableArray();

        self.addHeadquarter = function () {
            self.HeadquarterPerCompanies.push(new Item());
        };

        self.deleteHeadquarter = function (item) {
            self.HeadquarterPerCompanies.remove(item);
            if (self.HeadquarterPerCompanies().length === 0)
                self.addHeadquarter();
        };

        self.search = function () {
            
            var id = document.getElementById('IdentificationNumber').value;
            var input = {
                ruc: id
            };
            
            $.ajax({
                url: '/Sunat/Sunat/FindByRuc',
                type: 'GET',
                contentType: 'application/json; charset=utf-8',                
                data: input,
                dataType: "json",
                success: function (data) {

                    if (self.HeadquarterPerCompanies().length > 0)
                        self.HeadquarterPerCompanies.removeAll();
                                        
                    self.Name(data.Name);
                    self.Address(data.Address);                   
                    self.PathLogo("");
                    self.PhoneNumber("");
                    self.ContactName("");
                    self.Mail("");
                    self.District(data.District);
                    self.PhoneCompany("");

                    if (data.SunatHeadquarters.length > 0) {                        
                        for (var i = 0; i < data.SunatHeadquarters.length; i++) {
                            self.HeadquarterPerCompanies.push(new SetHeadquarter(data.SunatHeadquarters[i]));
                        }
                    }

                }
            }).fail(
                function (xhr, textStatus, err) {
                    alert(err);
                });
        };

        self.errors = ko.validation.group(this, { deep: true, observable: false });

        self.submit = function () {
            if (self.errors().length === 0)
                return true;

            self.errors.showAllMessages();
            return false;
        };




    }


    function SetHeadquarter(data) {

        var self = this;

        self.Department = ko.observable(data.Department).extend({ required: { message: '' } });
        self.Province = ko.observable(data.Province).extend({ required: { message: '' } });
        self.District = ko.observable(data.District).extend({ required: { message: '' } });        
        self.HeadquarterAddress = ko.observable(data.HeadquarterAddress).extend({ required: false });
        self.HeadquarterPhone = ko.observable(data.HeadquarterPhone).extend({ required: false });

    }

    function Item() {
        var self = this;

        self.Department = ko.observable("").extend({ required: { message: '' } });
        self.Province = ko.observable("").extend({ required: { message: '' } });
        self.District = ko.observable("").extend({ required: { message: '' } });
        self.HeadquarterAddress = ko.observable("").extend({ required: { message: '' } });
        self.HeadquarterPhone = ko.observable("").extend({ required: { message: '' } });
    }
});