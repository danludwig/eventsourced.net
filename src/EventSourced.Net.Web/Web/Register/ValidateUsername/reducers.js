import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/reducers/standardApi'
import { VALIDATE_USERNAME } from './actions'

export default handleActions({
  [VALIDATE_USERNAME.SENT]: standardApi.sent,
  [VALIDATE_USERNAME.FAIL]: standardApi.fail,
  [VALIDATE_USERNAME.DONE]: standardApi.done,
}, standardApi.initialState)
