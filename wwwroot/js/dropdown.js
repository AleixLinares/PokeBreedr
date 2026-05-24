window.dropdownInterop = {

    listeners: {},

    registerOutsideClick: function (id, element, dotnetHelper) {

        const handler = function (event) {

            if (!element || !element.contains) {
                return;
            }

            if (!element.contains(event.target)) {

                dotnetHelper.invokeMethodAsync('CloseDropdown')
                    .catch(() => { });
            }
        };

        document.addEventListener('click', handler);

        this.listeners[id] = handler;
    },

    unregisterOutsideClick: function (id) {

        const handler = this.listeners[id];

        if (handler) {

            document.removeEventListener('click', handler);

            delete this.listeners[id];
        }
    }
};