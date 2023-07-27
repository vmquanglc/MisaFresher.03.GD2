import MSEnum from "../enum/enum";
import emitter from "tiny-emitter/instance";

export default function processError(error) {
  try {
    let message = error.data.UserMessage;

    switch (error.status) {
      case MSEnum.HttpStatusCode.ServerError:
        emitter.emit("showDialog", MSEnum.DialogType.Error, message);
        break;

      case MSEnum.HttpStatusCode.BadRequest:
        emitter.emit("showDialog", MSEnum.DialogType.Error, message);
        break;

      case MSEnum.HttpStatusCode.NotFound:
        emitter.emit("showDialog", MSEnum.DialogType.Error, message);
        break;

      default:
        console.log("Mã lỗi mới: ", error);
        break;
    }
  } catch (e) {
    console.log("Lỗi nghiêm trọng: ", e);
  }
}
