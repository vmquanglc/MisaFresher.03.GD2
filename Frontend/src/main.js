import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import commonVal from "./js/common/value";
import axios from "axios";
import "./css/main.css";
import BaseButton from "./components/base/BaseButton.vue";
import BaseTextfield from "./components/base/BaseTextfield.vue";
import BaseCheckbox from "./components/base/BaseCheckbox.vue";
import BaseCombobox from "./components/base/BaseCombobox.vue";
import BaseComboboxMultiSelect from "./components/base/BaseComboboxMultiSelect.vue";
import BaseComboboxTable from "./components/base/BaseComboboxTable.vue";
import BaseDatepicker from "./components/base/BaseDatepicker.vue";
import BaseDialog from "./components/base/BaseDialog.vue";
import BaseLoader from "./components/base/BaseLoader.vue";
import BaseNotibox from "./components/base/BaseNotibox.vue";
import BaseRadiogroup from "./components/base/BaseRadiogroup.vue";
import BaseSelectbox from "./components/base/BaseSelectbox.vue";
import BaseToastbox from "./components/base/BaseToastbox.vue";

import lang from "./js/resources/lang";
import setUpDirective from "./js/directive/directive";
var Emitter = require("tiny-emitter");
const emitter = new Emitter();

const app = createApp(App);

setUpDirective(app);

app.component("BaseButton", BaseButton);
app.component("BaseTextfield", BaseTextfield);
app.component("BaseCheckbox", BaseCheckbox);
app.component("BaseComboboxMultiSelect", BaseComboboxMultiSelect);
app.component("BaseCombobox", BaseCombobox);
app.component("BaseComboboxTable", BaseComboboxTable);
app.component("BaseDatepicker", BaseDatepicker);
app.component("BaseDialog", BaseDialog);
app.component("BaseLoader", BaseLoader);
app.component("BaseNotibox", BaseNotibox);
app.component("BaseRadiogroup", BaseRadiogroup);
app.component("BaseSelectbox", BaseSelectbox);
app.component("BaseToastbox", BaseToastbox);

app.provide("$lang", lang);
app.provide("$emitter", emitter);
app.provide("$common", commonVal);
app.provide("$axios", axios);

app.use(router);
app.mount("#app");
