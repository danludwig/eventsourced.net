import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../Shared/ValidationSummary'
import ReduxFormPropTypes from '../Shared/propTypes/reduxForm'

const form = 'login'
const fields = ['login', 'password', 'returnUrl']
const config = { form, fields, validate, }

const Form = ({ handleSubmit, submitting, submitFailed, error, errors, fields: { login, password, }, }) => (
  <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
    <div className="form-group">
      <div className="col-md-4">
        <label className="control-label sr-only">Email address, phone number, or username</label>
        <input type="text" className="form-control" placeholder="Email address, phone number, or username" disabled={submitting} {...login} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-4">
        <label className="control-label sr-only">Password</label>
        <input type="password" className="form-control" placeholder="Password" disabled={submitting} {...password} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-10">
        <button type="submit" className="btn btn-default" disabled={submitting}>Log in</button>
      </div>
    </div>
    <ValidationSummary errors={{ error, ...errors }} visible={submitFailed && !submitting} />
  </form>
)

Form.propTypes = {
  ...ReduxFormPropTypes.handleSubmit,
  ...ReduxFormPropTypes.submitting,
  ...ReduxFormPropTypes.submitFailed,
  ...ReduxFormPropTypes.error,
  ...ReduxFormPropTypes.formName,
  errors: React.PropTypes.shape({
    login: React.PropTypes.string,
    password: React.PropTypes.string,
  }).isRequired,
  fields: React.PropTypes.shape({
    login: ReduxFormPropTypes.field.isRequired,
    password: ReduxFormPropTypes.field.isRequired,
    returnUrl: ReduxFormPropTypes.field.isRequired,
  }).isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)
