import { reduxForm } from 'redux-form'
import validate from './validation'
import ValidationSummary from '../../Shared/ValidationSummary'
import ReduxFormPropTypes from '../../Shared/propTypes/reduxForm'

const form = 'register'
const fields = ['emailOrPhone', 'returnUrl']
const config = { form, fields, validate, }

const Form = ({ handleSubmit, submitting, submitFailed, error, errors, fields: { emailOrPhone }, }) => (
  <form method="post" className="form-horizontal" role="form" onSubmit={handleSubmit}>
    <div className="form-group">
      <div className="col-md-4">
        <label className="control-label sr-only">Email address or phone number</label>
        <input type="text" name="emailOrPhone" className="form-control" placeholder="Email address or phone number" disabled={submitting} {...emailOrPhone} />
      </div>
    </div>
    <div className="form-group">
      <div className="col-md-10">
        <button type="submit" className="btn btn-default" disabled={submitting}>Register</button>
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
    emailOrPhone: React.PropTypes.string,
  }).isRequired,
  fields: React.PropTypes.shape({
    emailOrPhone: ReduxFormPropTypes.field.isRequired,
    returnUrl: ReduxFormPropTypes.field.isRequired,
  }).isRequired,
}

const select = () => ({ formName: form, })
export default reduxForm(config, select)(Form)
