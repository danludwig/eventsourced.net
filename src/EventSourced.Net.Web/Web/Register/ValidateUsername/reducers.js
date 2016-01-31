import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/standardApi'
import { VALIDATE_USERNAME } from './actions'

export default handleActions({
  [VALIDATE_USERNAME.SENT]: standardApi.reducers.sent,
  [VALIDATE_USERNAME.FAIL]: standardApi.reducers.fail,
  [VALIDATE_USERNAME.DONE]: standardApi.reducers.done,
}, standardApi.initialState)
