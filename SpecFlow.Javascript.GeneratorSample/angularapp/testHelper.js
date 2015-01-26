
(function () {
    var scenarioContext = function () {
        var self = this;
        inject(function (_$injector_) {
            self.$injector = _$injector_;
            self.$controller = self.$injector.get('$controller');
            self.$httpBackend = self.$injector.get('$httpBackend');
            self.$rootScope = self.$injector.get('$rootScope');
        });
    };

    window.getContext = function () {
        return new scenarioContext();
    }
})();