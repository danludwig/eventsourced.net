import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/standardApi'
import { REGISTER } from './actions'

export default handleActions({
  [REGISTER.SENT]: standardApi.reducers.sent,
  [REGISTER.FAIL]: standardApi.reducers.fail,
  [REGISTER.DONE]: standardApi.reducers.done,
}, standardApi.initialState)
