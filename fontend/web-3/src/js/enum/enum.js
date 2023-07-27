const MSEnum = {
  HttpStatusCode: {
    Success: 200,
    CreatedSuccess: 201,
    NoContent: 204,
    BadRequest: 400,
    NoAuthen: 401,
    NotFound: 404,
    ServerError: 500,
  },
  DialogType: {
    Delete: 1,
    DeleteSelected: 4,
    Alert: 2,
    Error: 3,
    AlertFormChanged: 5,
    FeatureNotSupported: 6,
  },
  NoticeType: {
    Success: 0,
    Error: 1,
    Warning: 2,
    Information: 3,
  },
  DialogAnswer: {
    Yes: 0,
    No: 1,
    Cancel: 2,
  },
  Gender: {
    Male: 0,
    Female: 1,
    Other: 2,
  },
  NumberOfEmployeesPerPage: {
    Max: 200,
    Min: 10,
  },
};

export default MSEnum;
