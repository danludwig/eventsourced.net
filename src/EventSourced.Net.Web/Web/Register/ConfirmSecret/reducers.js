import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/reducers/standardApi'
import { VERIFY } from './actions'

export default handleActions({
  [VERIFY.SENT]: standardApi.sent,
  [VERIFY.FAIL]: standardApi.fail,
  [VERIFY.DONE]: standardApi.done,
}, standardApi.initialState)
