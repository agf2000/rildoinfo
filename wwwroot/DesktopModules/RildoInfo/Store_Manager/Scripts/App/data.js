
// UnitTypes transport
my.unitTypesTransport = {
    read: {
        url: '/desktopmodules/riw/api/unittypes/getUnitTypes'
    },
    parameterMap: function (data, type) {
        return {
            portalId: _portalID,
            isDeleted: 'false'
        };
    }
};

// UnitTypes datasource
my.unitTypesData = new kendo.data.DataSource({
    transport: my.unitTypesTransport,
    sort: { field: "UnitTypeTitle", dir: "ASC" },
    schema: {
        model: {
            id: 'UnitTypeId',
            fields: {
                ModifiedDate: { type: "date", format: "{0:dd/MM/yyyy}" }
            }
        }
    }
});

my.accountsTransport = {
    read: {
        url: '/desktopmodules/riw/api/accounts/getAccounts?portalId=' + _portalID
    }
};

my.accountsData = new kendo.data.DataSource({
    transport: my.accountsTransport,
    sort: {
        field: 'AccountName',
        dir: 'ASC'
    },
    schema: {
        model: {
            ud: 'AccountId'
        }
    }
});

my.originsTransport = {
    read: {
        url: '/desktopmodules/riw/api/origins/getOrigins?portalId=' + _portalID
    }
};

my.originsData = new kendo.data.DataSource({
    transport: my.originsTransport,
    sort: {
        field: 'OriginName',
        dir: 'ASC'
    },
    schema: {
        model: {
            ud: 'OriginId'
        }
    }
});

