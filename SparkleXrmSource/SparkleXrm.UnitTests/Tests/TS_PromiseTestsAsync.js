/// <reference path="sparklexrm.d.ts" />
/// <reference path="node_modules/@types/qunit/index.d.ts" />
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var tsTests;
(function (tsTests) {
    var Entity = SparkleXrm.Sdk.Entity;
    var Guid = SparkleXrm.Sdk.Guid;
    var XrmService = SparkleXrm.Sdk.XrmService;
    var TestPromiseWebApiAsync = /** @class */ (function () {
        function TestPromiseWebApiAsync() {
        }
        // This is an async TS version of the TS_CrudTests
        // We mark the method as async and use the await keyword so that Typescript will automatically resolve promises
        TestPromiseWebApiAsync.Create_01 = function (assert) {
            return __awaiter(this, void 0, void 0, function () {
                var done, contact, name, id, ex_1;
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0:
                            debugger;
                            assert.expect(1);
                            done = assert.async();
                            contact = new Entity("contact");
                            name = "Test " + Date.now.toString();
                            contact.setAttributeValue("lastname", name);
                            _a.label = 1;
                        case 1:
                            _a.trys.push([1, 3, , 4]);
                            return [4 /*yield*/, XrmService.create(contact)];
                        case 2:
                            id = _a.sent();
                            assert.ok(id != null, id.value);
                            contact.id = id.value;
                            return [3 /*break*/, 4];
                        case 3:
                            ex_1 = _a.sent();
                            console.log(ex_1.message);
                            return [3 /*break*/, 4];
                        case 4: return [4 /*yield*/, XrmService.delete_(contact.logicalName, new Guid(contact.id))];
                        case 5:
                            _a.sent();
                            done();
                            return [2 /*return*/];
                    }
                });
            });
        };
        return TestPromiseWebApiAsync;
    }());
    tsTests.TestPromiseWebApiAsync = TestPromiseWebApiAsync;
    QUnit.module("TS_PromiseTests.Async");
    QUnit.test("TSPromise.Async.Create_01", TestPromiseWebApiAsync.Create_01);
})(tsTests || (tsTests = {}));
//# sourceMappingURL=/Webresources/sparkle_/unittests/TS_PromiseTestsAsync.js.map