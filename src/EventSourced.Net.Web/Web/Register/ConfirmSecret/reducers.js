import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/standardApi'
import { VERIFY } from './actions'

export default handleActions({
  [VERIFY.SENT]: standardApi.reducers.sent,
  [VERIFY.FAIL]: standardApi.reducers.fail,
  [VERIFY.DONE]: standardApi.reducers.done,
}, standardApi.initialState)
