import { handleActions } from 'redux-actions'
import standardApi from '../../Shared/reducers/standardApi'
import { REGISTER } from './actions'

export default handleActions({
  [REGISTER.SENT]: standardApi.sent,
  [REGISTER.FAIL]: standardApi.fail,
  [REGISTER.DONE]: standardApi.done,
}, standardApi.initialState)
